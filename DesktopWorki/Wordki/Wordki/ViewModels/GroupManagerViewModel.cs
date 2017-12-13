using Repository.Models;
using Repository.Models.Enums;
using Repository.Models.Language;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Util;
using Util.Collections;
using Util.Serializers;
using Wordki.Database;
using Wordki.Helpers;
using Wordki.Helpers.WordComparer;
using Wordki.Models;
using Wordki.Models.Lesson;
using Wordki.Models.LessonScheduler;
using Wordki.ViewModels.Dialogs;

namespace Wordki.ViewModels
{
    public enum SortDirection
    {
        Increasing,
        Decreasing
    }

    public class GroupManagerViewModel : ViewModelBase
    {

        private static readonly object _groupsLock = new object();
        private GroupItem _selectedItem;
        private string _translationDirectionLabel;
        private string _allWordsLabel;
        private string _timeOutLabel;
        private ObservableCollection<GroupItem> _itemList;
        private ObservableCollection<double> _values;
        private double _maxValue;

        #region Properties

        public ICommand StartLessonCommand { get; set; }
        public ICommand EditGroupCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ShowWordsCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand DrawerSignClickCommand { get; set; }
        public ICommand TranslationDirectionChangedCommand { get; set; }
        public ICommand AllWordsCommand { get; set; }
        public ICommand ShowPlotCommand { get; set; }
        public ICommand FinishLessonCommand { get; set; }
        public ICommand SortDirectionCommand { get; private set; }

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
                if (_selectedItem == value)
                {
                    return;
                }
                _selectedItem = value;
                OnPropertyChanged();
                RefreshInfo();
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

        public Settings Settings { get; set; }
        public IList<GroupItem> SelectionList { get; set; }
        public GroupInfo GroupInfo { get; set; }

        #endregion

        public GroupManagerViewModel()
        {
            Settings = Settings.GetSettings();
            GroupInfo = new GroupInfo();
            ActivateCommond();
            Values = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
            ItemsList = new ObservableCollection<GroupItem>();
            SelectionList = new List<GroupItem>();
            BindingOperations.EnableCollectionSynchronization(ItemsList, _groupsLock);
        }

        #region Commands

        private void ActivateCommond()
        {
            StartLessonCommand = new Util.BuilderCommand(StartLesson);
            EditGroupCommand = new Util.BuilderCommand(EditGroup);
            BackCommand = new Util.BuilderCommand(BackAction);
            ShowWordsCommand = new Util.BuilderCommand(ShowWords);
            SelectionChangedCommand = new Util.BuilderCommand(SelectionChanged);
            DrawerSignClickCommand = new Util.BuilderCommand(DrawerSignClick);
            TranslationDirectionChangedCommand = new Util.BuilderCommand(TranslationDirectionChanged);
            AllWordsCommand = new Util.BuilderCommand(AllWords);
            ShowPlotCommand = new Util.BuilderCommand(ShowPlot);
            FinishLessonCommand = new Util.BuilderCommand(FinishLesson);
        }

        private void FinishLesson(object obj)
        {
            ISerializer<Lesson> serializer = new BinarySerializer<Lesson>
            {
                Settings = new BinarySerializerSettings
                {
                    Path = "lesson",
                    RemoveAfterRead = true,
                }
            };
            Lesson lesson;
            try
            {
                //lesson = serializer.Read();
            }
            catch (Exception e)
            {
                LoggerSingleton.LogError("Błąd w czasie deserializacji objektu - {0}", e.Message);
                return;
            }
            //PackageStore.Put(0, lesson);
            Switcher.Switch(Switcher.State.TeachTyping);
        }

        private void EditGroup(object obj)
        {
            IGroup lSelectedGroup = SelectedItem.Group;
            PackageStore.Put(0, lSelectedGroup);
            Switcher.Switch(Switcher.State.Builder);
        }

        private async void AllWords(object obj)
        {
            IUserManager userManager = UserManagerSingleton.Instence;
            userManager.User.AllWords = !userManager.User.AllWords;
            await userManager.UpdateAsync();
            SetAllWordsLabel();
        }

        private void ShowPlot(object obj)
        {
            if (SelectedItem == null) return;
            PackageStore.Put(0, SelectedItem.Group.Id);
        }

        private async void TranslationDirectionChanged(object obj)
        {
            IUserManager userManager = UserManagerSingleton.Instence;
            switch (userManager.User.TranslationDirection)
            {
                case TranslationDirection.FromFirst:
                    userManager.User.TranslationDirection = TranslationDirection.FromSecond;
                    break;
                case TranslationDirection.FromSecond:
                    userManager.User.TranslationDirection = TranslationDirection.FromFirst;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            await userManager.UpdateAsync();
            SetTranslationDirectionLabel();
        }

        private void DrawerSignClick(object obj)
        {
            try
            {
                if (obj == null)
                {
                    LoggerSingleton.LogError("{0} - {1}", "DrawerSignClick", "obj == null");
                }
            }
            catch (Exception lException)
            {
                LoggerSingleton.LogError("{0} - {1}", "DrawerSignClick", lException.Message);
            }
        }
        private void SelectionChanged(object obj)
        {
            if (obj == null)
                return;
            IList lList = (obj as IList);
            if (lList == null)
                return;
            SelectionList.Clear();
            SelectionList.AddRange(lList.Cast<GroupItem>());
            RefreshInfo();
        }

        private void ShowWords(object obj)
        {
            if (SelectedItem == null) return;
            PackageStore.Put(0, SelectedItem.Group.Id);
            Switcher.Switch(Switcher.State.Words);
        }

        private void BackAction(object obj)
        {
            Back();
            Switcher.Back();
        }

        #endregion

        public override void InitViewModel()
        {
            Task.Run(() =>
            {
                ItemsList.Clear();
                ILessonScheduler scheduler = new NewLessonScheduler()
                {
                    Initializer = new LessonSchedulerInitializer2(new List<int>() { 1, 1, 2, 4, 7 })
                    {
                        TranslationDirection = TranslationDirection.FromFirst,
                    },
                };
                foreach (GroupItem groupItem in DatabaseSingleton.Instance.Groups.Select(group => new GroupItem(group)))
                {
                    groupItem.Color = scheduler.GetColor(groupItem.Group);
                    groupItem.NextRepeat = Math.Max(scheduler.GetTimeToLearn(groupItem.Group), 0);
                    ItemsList.Add(groupItem);
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (ItemsList.Count > 0)
                    {
                        SelectedItem = ItemsList.First();
                    }
                });
            });
            SetTranslationDirectionLabel();
            SetAllWordsLabel();
            SetEndLessonButton();
        }

        public override void Back()
        {

        }

        private void RefreshInfo()
        {
            List<double> lDrawersCount = new List<double>();
            LanguageType lang1 = SelectedItem.Group.Language1;
            LanguageType lang2 = SelectedItem.Group.Language2;
            DateTime lLastRepeat = DateTime.MinValue;
            for (int i = 0; i < 5; i++)
                lDrawersCount.Add(0);
            if (SelectionList == null)
                return;
            foreach (GroupItem item in SelectionList)
            {
                lang1 = lang1 != item.Group.Language1 && lang1 != LanguageType.Default ? LanguageType.Default : lang1;
                lang2 = lang2 != item.Group.Language2 && lang2 != LanguageType.Default ? LanguageType.Default : lang2;
                foreach (Word lWord in item.Group.Words)
                {
                    lDrawersCount[lWord.Drawer]++;
                }
                DateTime itemDateTime = item.Group.Results.Max(x => x.DateTime);
                if (itemDateTime > lLastRepeat)
                {
                    lLastRepeat = itemDateTime;
                }
            }
            string groupName = SelectionList.Count == 1 ? SelectionList.First().Group.Name : " ";
            int lWordsCount = SelectionList.Sum(x => x.Group.Words.Count);
            int lVisibilitiesCount = SelectionList.Sum(x => x.Group.Words.Count(y => y.Visible));
            int lRepeatsCount = SelectionList.Sum(x => x.Group.Results.Count(y => y.TranslationDirection == UserManagerSingleton.Instence.User.TranslationDirection));
            MaxValue = lDrawersCount.Sum();
            Values = new ObservableCollection<double>(lDrawersCount);
            GroupInfo.SetValue(groupName, lWordsCount, lRepeatsCount, lLastRepeat, lVisibilitiesCount,
                new BitmapImage(new Uri(LanguageIconManager.GetPathCircleFlag(LanguageFactory.GetLanguage(lang1)))),
                new BitmapImage(new Uri(LanguageIconManager.GetPathCircleFlag(LanguageFactory.GetLanguage(lang2)))));
        }

        private void SetTranslationDirectionLabel()
        {
            switch (UserManagerSingleton.Instence.User.TranslationDirection)
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
            AllWordsLabel = UserManagerSingleton.Instence.User.AllWords ? "Wszystkie słowa" : "Tylko widoczne";
        }

        private void SetEndLessonButton()
        {
            ISerializer<Lesson> serializer = new BinarySerializer<Lesson>
            {
                Settings = new BinarySerializerSettings
                {
                    Path = "lesson",
                    RemoveAfterRead = false,
                }
            };
            //Lesson serializedLesson = serializer.Read();
            //if (serializedLesson == null)
            //{
            //    EndLessonButtonVisibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    EndLessonButtonVisibility = Visibility.Visible;
            //}
        }

        private IEnumerable<IWord> GetWordListFromSelectedGroups()
        {
            List<IWord> lWords = new List<IWord>();
            try
            {
                foreach (GroupItem lItem in SelectionList)
                {
                    IGroup lGroup = lItem.Group;
                    lWords.AddRange(lGroup.Words);
                }
            }
            catch (Exception lException)
            {
                LoggerSingleton.LogError("{0} - {1}", "GetWordListFromSelectedGroups", lException.Message);
            }
            return lWords;
        }

        private IEnumerable<IWord> GetWordListRandom(int pCount)
        {
            var groupList = DatabaseSingleton.Instance.Groups;
            Random random = new Random();
            for (int i = 0; i < pCount; i++)
            {
                IGroup group = groupList[random.Next(groupList.Count - 1)];
                yield return group.Words[random.Next(group.Words.Count - 1)];
            }
        }

        private IEnumerable<IWord> GetWordListBest(int pCount)
        {
            List<IWord> temp = new List<IWord>();
            foreach (Group group in DatabaseSingleton.Instance.Groups)
            {
                temp.AddRange(group.Words);
            }
            IEnumerable<IWord> result = temp.Where(x => x.Drawer == 4);
            result = result.ToList().Shuffle();
            return result.Take(pCount);
        }

        private IEnumerable<IWord> GetAllToTeach()
        {
            IUser user = UserManagerSingleton.Instence.User;
            ILessonScheduler scheduler = new NewLessonScheduler()
            {
                Initializer = new LessonSchedulerInitializer2(new List<int>() { 1, 1, 2, 4, 7 })
                {
                    TranslationDirection = user.TranslationDirection,
                },
            };
            foreach (IGroup group in DatabaseSingleton.Instance.Groups)
            {
                if (scheduler.GetTimeToLearn(group) > 0)
                {
                    continue;
                }
                foreach(IWord word in group.Words.Where(x => x.Visible || user.AllWords))
                {
                    yield return word;
                }
            }
        }

        private void StartLesson(object obj)
        {
            LessonType lLessonType = (LessonType)obj;
            try
            {
                Lesson lesson;
                Switcher.State stateToStart;
                switch (lLessonType)
                {
                    case LessonType.FiszkiLesson:
                        {
                            lesson = new FiszkiLesson(GetWordListFromSelectedGroups());
                            stateToStart = Switcher.State.TeachFiszki;
                        }
                        break;
                    case LessonType.TypingLesson:
                        {
                            lesson = new TypingLesson(GetWordListFromSelectedGroups());
                            stateToStart = Switcher.State.TeachTyping;
                        }
                        break;
                    case LessonType.IntensiveLesson:
                        {
                            lesson = new IntensiveLesson(GetWordListFromSelectedGroups());
                            stateToStart = Switcher.State.TeachTyping;
                        }
                        break;
                    case LessonType.RandomLesson:
                        {
                            lesson = new RandomLesson(GetWordListRandom(60));
                            stateToStart = Switcher.State.TeachTyping;
                        }
                        break;
                    case LessonType.BestLesson:
                        {
                            lesson = new BestLesson(GetWordListBest(60));
                            stateToStart = Switcher.State.TeachTyping;
                        }
                        break;
                    case LessonType.AllToTeach:
                        {
                            lesson = new TypingLesson(GetAllToTeach());
                            stateToStart = Switcher.State.TeachTyping;
                        }
                        break;
                    default:
                        {
                            LoggerSingleton.LogError("{0} - {1} - {2}", "Blad w startowaniu lekcji", "Blad w przekazanym parametrze", lLessonType);
                            return;
                        }
                }
                lesson.WordComparer = new WordComparer();
                lesson.WordComparer.Settings = new WordComparerSettings();
                lesson.WordComparer.Settings.WordSeparator = ',';
                lesson.WordComparer.Settings.NotCheckers.Add(new LetterCaseNotCheck());
                lesson.WordComparer.Settings.NotCheckers.Add(new SpaceNotCheck());
                lesson.WordComparer.Settings.NotCheckers.Add(new Utf8NotCheck());
                IUser user = UserManagerSingleton.Instence.User;
                ILessonSettings lessonSettings = new LessonSettings()
                {
                    AllWords = user.AllWords,
                    TranslationDirection = user.TranslationDirection,
                    Timeout = user.Timeout,
                };
                lesson.LessonSettings = lessonSettings;
                lesson.InitLesson();
                if (lesson.BeginWordsList.Count == 0)
                {
                    InteractionProvider.IInteractionProvider provider = new InteractionProvider.SimpleInfoProvider
                    {
                        ViewModel = new InfoDialogViewModel
                        {
                            ButtonLabel = "Ok",
                            Message = "Brak słów do nauki"
                        }
                    };
                    provider.Interact();
                    return;
                }
                PackageStore.Put(0, lesson);
                Switcher.Switch(stateToStart);
            }
            catch (Exception lException)
            {
                LoggerSingleton.LogError("{0} - {1} - {2}", "Blad w startowaniu lekcji", lLessonType.ToString(), lException.Message);
            }
        }

        public override void Loaded()
        {
        }

        public override void Unloaded()
        {
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
                LastRepeat = Convert.ToString((int)(DateTime.Now - pLastRepeat).TotalDays);
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

