using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Repository.Models.Enums;
using Wordki.Helpers;
using Wordki.Models;
using Wordki.Models.Lesson;
using Wordki.Models.Lesson.WordComparer;
using Wordki.Models.LessonScheduler;
using Wordki.Models.LessonScheduler.LessonScheduleInitializer;

namespace Wordki.ViewModels
{
    public class GroupManagerViewModel : ViewModelBase
    {

        private static readonly object GroupsLock = new object();
        private GroupItem _selectedItem;
        private string _translationDirectionLabel;
        private string _allWordsLabel;
        private string _timeOutLabel;
        private ObservableCollection<GroupItem> _itemList;
        private ObservableCollection<double> _values;
        private double _maxValue;

        #region Properties

        public BuilderCommand StartTypingLessonCommand { get; set; }
        public BuilderCommand StartReapetLessonCommand { get; set; }
        public BuilderCommand StartRandomLessonCommand { get; set; }
        public BuilderCommand StartBestLessonCommand { get; set; }
        public BuilderCommand EditGroupCommand { get; set; }
        public BuilderCommand BackCommand { get; set; }
        public BuilderCommand ShowWordsCommand { get; set; }
        public BuilderCommand SelectionChangedCommand { get; set; }
        public BuilderCommand DrawerSignClickCommand { get; set; }
        public BuilderCommand TranslationDirectionChangedCommand { get; set; }
        public BuilderCommand AllWordsCommand { get; set; }
        public BuilderCommand ShowPlotCommand { get; set; }
        public BuilderCommand FinishLessonCommand { get; set; }

        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (Math.Abs(_maxValue - value) < 0.1) return;
                _maxValue = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<double> Values
        {
            get { return _values; }
            set
            {
                if (_values == value) return;
                _values = value;
                OnPropertyChanged();
            }
        }
        public GroupItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged();
                    RefreshInfo();
                }
            }
        }
        public string TranslationDirectionLabel
        {
            get { return _translationDirectionLabel; }
            set
            {
                if (_translationDirectionLabel != value)
                {
                    _translationDirectionLabel = value;
                    OnPropertyChanged();
                }
            }
        }
        public string AllWordsLabel
        {
            get { return _allWordsLabel; }
            set
            {
                if (_allWordsLabel != value)
                {
                    _allWordsLabel = value;
                    OnPropertyChanged();
                }
            }
        }
        public string TimeOutLabel
        {
            get { return _timeOutLabel; }
            set
            {
                if (_timeOutLabel != value)
                {
                    _timeOutLabel = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<GroupItem> ItemsList
        {
            get { return _itemList; }
            set
            {
                if (_itemList != value)
                {
                    _itemList = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _endLessonButtonVisibility;
        public Visibility EndLessonButtonVisibility
        {
            get { return _endLessonButtonVisibility; }
            set
            {
                if (_endLessonButtonVisibility == value) return;
                _endLessonButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        public Settings Settings { get; set; }
        public IList SelectionList { get; set; }
        public GroupInfo GroupInfo { get; set; }

        #endregion

        public GroupManagerViewModel()
        {
            Settings = Settings.GetSettings();
            GroupInfo = new GroupInfo();
            ActivateCommond();
            Values = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
            ItemsList = new ObservableCollection<GroupItem>();
            BindingOperations.EnableCollectionSynchronization(ItemsList, GroupsLock);
        }

        #region Commands

        private void ActivateCommond()
        {
            StartTypingLessonCommand = new BuilderCommand(StartTypingLesson);
            StartReapetLessonCommand = new BuilderCommand(StartReapetLesson);
            StartRandomLessonCommand = new BuilderCommand(StartRandomLesson);
            StartBestLessonCommand = new BuilderCommand(StartBestLesson);
            EditGroupCommand = new BuilderCommand(EditGroup);
            BackCommand = new BuilderCommand(Back);
            ShowWordsCommand = new BuilderCommand(ShowWords);
            SelectionChangedCommand = new BuilderCommand(SelectionChanged);
            DrawerSignClickCommand = new BuilderCommand(DrawerSignClick);
            TranslationDirectionChangedCommand = new BuilderCommand(TranslationDirectionChanged);
            AllWordsCommand = new BuilderCommand(AllWords);
            ShowPlotCommand = new BuilderCommand(ShowPlot);
            FinishLessonCommand = new BuilderCommand(FinishLesson);
        }

        private void FinishLesson(object obj)
        {
            Lesson lesson;
            try
            {
                lesson = ObjectSerializer.ReadFromBinaryFile<Lesson>("lesson", true);
            }
            catch (Exception e)
            {
                Logger.LogError("Błąd w czasie deserializacji objektu - {0}", e.Message);
                return;
            }
            PackageStore.Put(0, lesson);
            Switcher.GetSwitcher().Switch(Switcher.State.Teach);
        }

        private void EditGroup(object obj)
        {
            Group lSelectedGroup = Database.GetDatabase().GetGroupById(SelectedItem.Group.Id);
            PackageStore.Put(0, lSelectedGroup);
            Switcher.GetSwitcher().Switch(Switcher.State.Builder);
        }

        private void AllWords(object obj)
        {
            Database.GetDatabase().User.AllWords = !Database.GetDatabase().User.AllWords;
            Database.GetDatabase().UpdateUser(Database.GetDatabase().User);
            SetAllWordsLabel();
        }

        private void ShowPlot(object obj)
        {
            if (SelectedItem == null) return;
            PackageStore.Put(0, SelectedItem.Group.Id);
        }

        private void TranslationDirectionChanged(object obj)
        {
            switch (Database.GetDatabase().User.TranslationDirection)
            {
                case TranslationDirection.FromFirst:
                    Database.GetDatabase().User.TranslationDirection = TranslationDirection.FromSecond;
                    break;
                case TranslationDirection.FromSecond:
                    Database.GetDatabase().User.TranslationDirection = TranslationDirection.FromFirst;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Database.GetDatabase().UpdateUser(Database.GetDatabase().User);
            SetTranslationDirectionLabel();
        }

        private void DrawerSignClick(object obj)
        {
            try
            {
                if (obj == null)
                {
                    Logger.LogError("{0} - {1}", "DrawerSignClick", "obj == null");
                }
            }
            catch (Exception lException)
            {
                Logger.LogError("{0} - {1}", "DrawerSignClick", lException.Message);
            }
        }
        private void SelectionChanged(object obj)
        {
            if (obj == null)
                return;
            IList lList = (obj as IList);
            if (lList == null)
                return;
            SelectionList = lList;
            RefreshInfo();
        }

        private void ShowWords(object obj)
        {
            if (SelectedItem == null) return;
            PackageStore.Put(0, SelectedItem.Group.Id);
            Switcher.GetSwitcher().Switch(Switcher.State.Words);
        }

        private void Back(object obj)
        {
            Back();
            Switcher.GetSwitcher().Back();
        }

        private void StartReapetLesson(object obj)
        {
            StartLesson(LessonType.IntensiveLesson);
        }

        private void StartTypingLesson(object obj)
        {
            StartLesson(LessonType.TypingLesson);
        }

        private void StartRandomLesson(object obj)
        {
            StartLesson(LessonType.RandomLesson);
        }

        private void StartBestLesson(object obj)
        {
            StartLesson(LessonType.BestLesson);
        }
        #endregion

        public override void InitViewModel()
        {
            Task.Run(() =>
            {
                ItemsList.Clear();
                ILessonScheduler scheduler = new LessonScheduler(new SimpleLessonScheduleInitializer());
                foreach (GroupItem groupItem in Database.GetDatabase().GroupsList.Select(group => new GroupItem(group)))
                {
                    groupItem.Color = scheduler.GetColor(groupItem.Group.GetLastResult());
                    groupItem.NextRepeat = scheduler.GetTimeToLearn(groupItem.Group.ResultsList);
                    ItemsList.Add(groupItem);
                }
            });
            SetTranslationDirectionLabel();
            SetTimeOutLabel();
            SetAllWordsLabel();
            SetEndLessonButton();
        }

        public override void Back()
        {

        }

        private void RefreshInfo()
        {
            try
            {
                List<double> lDrawersCount = new List<double>();
                int lVisibilitiesCount = 0;
                int lWordsCount = 0;
                int lRepeatsCount = 0;
                BitmapImage lLanguage1Flag = null;
                BitmapImage lLanguage2Flag = null;
                DateTime lLastRepeat = DateTime.MinValue;
                StringBuilder lGroupNameBuilder = new StringBuilder();
                for (int i = 0; i < 5; i++)
                    lDrawersCount.Add(0);
                if (SelectionList == null)
                    return;
                foreach (GroupItem lGroupItem in SelectionList)
                {
                    Group lGroup = Database.GetDatabase().GetGroupById(lGroupItem.Group.Id);
                    lLanguage1Flag = new BitmapImage(new Uri(LanguageIconManager.GetPathRectFlag(lGroup.Language1)));
                    lLanguage2Flag = new BitmapImage(new Uri(LanguageIconManager.GetPathRectFlag(lGroup.Language2)));
                    lGroupNameBuilder.Append(lGroup.Name).Append(" ");
                    lWordsCount += lGroup.WordsList.Count;
                    lRepeatsCount += Database.GetDatabase().GetResultsList(lGroupItem.Group.Id).Count;
                    Result lResult = Database.GetDatabase().GetLastResult(lGroupItem.Group.Id);
                    if (lResult != null)
                    {
                        DateTime lGroupLastResult = lResult.DateTime;
                        if (lLastRepeat.CompareTo(lGroupLastResult) < 0)
                        {
                            lLastRepeat = lGroupLastResult;
                        }
                    }
                    foreach (Word lWord in lGroup.WordsList)
                    {
                        lDrawersCount[lWord.Drawer]++;
                        if (lWord.Visible)
                            lVisibilitiesCount++;
                    }
                }
                MaxValue = lDrawersCount.Sum();
                Values = new ObservableCollection<double>(lDrawersCount);
                GroupInfo.SetValue(lGroupNameBuilder.ToString(), lWordsCount, lRepeatsCount, lLastRepeat, lVisibilitiesCount, lLanguage1Flag, lLanguage2Flag);
            }
            catch (Exception lException)
            {
                Logger.LogError("{0} - {1}", "RefreshInfo", lException.Message);
            }
        }

        private void SetTimeOutLabel()
        {
            TimeOutLabel = Database.GetDatabase().User.Timeout > 0 ? String.Format("Odliczanie {0} sekund", Database.GetDatabase().User.Timeout) : "Odliczanie wyłączone";
        }

        private void SetTranslationDirectionLabel()
        {
            switch (Database.GetDatabase().User.TranslationDirection)
            {
                case TranslationDirection.FromSecond:
                    {
                        TranslationDirectionLabel = "Z drugiego";
                    }
                    break;
                case TranslationDirection.FromFirst:
                    {
                        TranslationDirectionLabel = "Z pierwszego";
                    }
                    break;
            }
        }

        private void SetAllWordsLabel()
        {
            AllWordsLabel = Database.GetDatabase().User.AllWords ? "Wszystkie słowa" : "Tylko widoczne";
        }

        private void SetEndLessonButton()
        {
            Lesson serializedLesson = ObjectSerializer.ReadFromBinaryFile<Lesson>("lesson", false);
            if (serializedLesson == null)
            {
                EndLessonButtonVisibility = Visibility.Collapsed;
            }
            else
            {
                EndLessonButtonVisibility = Visibility.Visible;
            }
        }

        private IEnumerable<Word> GetWordListFromSelectedGroups()
        {
            List<Word> lWordsList = new List<Word>();
            try
            {
                foreach (GroupItem lItem in SelectionList)
                {
                    Group lGroup = Database.GetDatabase().GetGroupById(lItem.Group.Id);
                    lWordsList.AddRange(lGroup.WordsList);
                }
            }
            catch (Exception lException)
            {
                Logger.LogError("{0} - {1}", "GetWordListFromSelectedGroups", lException.Message);
            }
            return lWordsList;
        }

        private IEnumerable<Word> GetWordListRandom(int pCount)
        {
            var groupList = Database.GetDatabase().GroupsList;
            List<Word> wordList = new List<Word>();
            Random random = new Random();
            for (int i = 0; i < pCount; i++)
            {
                Group group = groupList[random.Next(groupList.Count - 1)];
                wordList.Add(group.WordsList[random.Next(group.WordsList.Count - 1)]);
            }
            return wordList;
        }

        private IEnumerable<Word> GetWordListBest(int pCount)
        {
            List<Word> temp = new List<Word>();
            foreach (Group group in Database.GetDatabase().GroupsList)
            {
                temp.AddRange(group.WordsList);
            }
            IEnumerable<Word> result = temp.Where(x => x.Drawer == 4);
            result = ListShuffle.Shuffle(result.ToList());
            return result.Take(pCount);
        }

        private void StartLesson(LessonType lLessonType)
        {
            try
            {
                Lesson lLesson;
                switch (lLessonType)
                {
                    case LessonType.TypingLesson:
                        {
                            lLesson = new TypingLesson(GetWordListFromSelectedGroups());
                        }
                        break;
                    case LessonType.IntensiveLesson:
                        {
                            lLesson = new IntensiveLesson(GetWordListFromSelectedGroups());
                        }
                        break;
                    case LessonType.RandomLesson:
                        {
                            lLesson = new RandomLesson(GetWordListRandom(60));
                        }
                        break;
                    case LessonType.BestLesson:
                        {
                            lLesson = new BestLesson(GetWordListBest(60));
                        }
                        break;
                    default:
                        {
                            Logger.LogError("{0} - {1} - {2}", "Blad w startowaniu lekcji", "Blad w przekazanym parametrze", lLessonType);
                            return;
                        }
                }
                lLesson.WordComparer = new WordComparer();
                lLesson.WordComparer.NotCheckers.Add(new LetterCaseNotCheck());
                lLesson.WordComparer.NotCheckers.Add(new SpaceNotCheck());
                lLesson.WordComparer.NotCheckers.Add(new Utf8NotCheck());

                lLesson.InitLesson();
                PackageStore.Put(0, lLesson);
                Switcher.GetSwitcher().Switch(Switcher.State.Teach);
            }
            catch (Exception lException)
            {
                Logger.LogError("{0} - {1} - {2}", "Blad w startowaniu lekcji", lLessonType.ToString(), lException.Message);
            }
        }



    }

    public class GroupInfo : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string pPropertyname = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pPropertyname));
            }
        }

        #endregion

        private const string FormatDate = "yyyy-MM-dd hh:mm:ss";

        private string _groupName;
        private int _wordsCount;
        private int _repeatCount;
        private string _lastRepeat;
        private int _visibilitiesCount;
        private BitmapImage _langauge1Flag;
        private BitmapImage _langauge2Flag;

        public int WordCount
        {
            get { return _wordsCount; }
            set
            {
                if (_wordsCount != value)
                {
                    _wordsCount = value;
                    OnPropertyChanged();
                }
            }
        }
        public int RepeatCount
        {
            get { return _repeatCount; }
            set
            {
                if (_repeatCount != value)
                {
                    _repeatCount = value;
                    OnPropertyChanged();
                }
            }
        }
        public string LastRepeat
        {
            get { return _lastRepeat; }
            set
            {
                if (_lastRepeat != value)
                {
                    _lastRepeat = value;
                    OnPropertyChanged();
                }
            }
        }
        public int VisibilitiesCount
        {
            get { return _visibilitiesCount; }
            set
            {
                if (_visibilitiesCount != value)
                {
                    _visibilitiesCount = value;
                    OnPropertyChanged();
                }
            }
        }
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                if (_groupName != value)
                {
                    _groupName = value;
                    OnPropertyChanged();
                }
            }
        }
        public BitmapImage Language1Flag
        {
            get { return _langauge1Flag; }
            set
            {
                if (!Equals(_langauge1Flag, value))
                {
                    _langauge1Flag = value;
                    OnPropertyChanged();
                }
            }
        }
        public BitmapImage Language2Flag
        {
            get { return _langauge2Flag; }
            set
            {
                if (!Equals(_langauge2Flag, value))
                {
                    _langauge2Flag = value;
                    OnPropertyChanged();
                }
            }
        }

        public void SetValue(string pGroupName, int pWordCount, int pRepeatCount, DateTime pLastRepeat, int pVisibilitiesCount, BitmapImage pLanguage1Flag, BitmapImage pLanguage2Flag)
        {
            GroupName = pGroupName;
            WordCount = pWordCount;
            RepeatCount = pRepeatCount;
            if (pRepeatCount != 0)
            {
                LastRepeat = pLastRepeat.ToString(FormatDate);
            }
            else
            {
                LastRepeat = "-";
            }
            VisibilitiesCount = pVisibilitiesCount;
            Language1Flag = pLanguage1Flag;
            Language2Flag = pLanguage2Flag;
        }
    }
}

