using WordkiModel;
using WordkiModel.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Util.Collections;
using Wordki.Commands;
using Wordki.Database;
using Wordki.Helpers;
using Wordki.Helpers.WordComparer;
using Wordki.Models;
using Wordki.Models.Lesson;
using Wordki.ViewModels.Dialogs;

namespace Wordki.ViewModels
{
    public class GroupManagerViewModel : ViewModelBase
    {
        private ObservableCollection<double> _values;
        private double _maxValue;

        #region Properties

        public ICommand StartLessonCommand { get; set; }
        public ICommand EditGroupCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ShowWordsCommand { get; set; }
        public ICommand TranslationDirectionChangedCommand { get; set; }
        public ICommand AllWordsCommand { get; set; }
        public ICommand SortDirectionCommand { get; private set; }
        public ICommand SelectionChangedCommand { get; private set; }

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
        private IGroup selectedItem;
        public IGroup SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem == value)
                {
                    return;
                }
                selectedItem = value;
                OnPropertyChanged();
                RefreshInfo();
            }
        }

        private IList<IGroup> selectedItems;
        public IList<IGroup> SelectedItems
        {
            get { return selectedItems; }
            set
            {
                if (selectedItems == value)
                {
                    return;
                }
                selectedItems = value;
                OnPropertyChanged();
            }
        }

        private IUser user;
        public IUser User
        {
            get { return user; }
            set
            {
                if (user == value)
                {
                    return;
                }
                user = value;
                OnPropertyChanged();
            }
        }


        public GroupInfo GroupInfo { get; set; }
        public IDatabase Database { get; set; }
        #endregion

        public GroupManagerViewModel()
        {
            Database = DatabaseSingleton.Instance;
            GroupInfo = new GroupInfo();
            Values = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
            SelectedItems = new List<IGroup>();

            StartLessonCommand = new Util.BuilderCommand((obj) => StartLesson((LessonType)obj));
            EditGroupCommand = new Util.BuilderCommand((obj) => EditGroup(obj as IGroup));
            BackCommand = new Util.BuilderCommand(BackAction);
            ShowWordsCommand = new Util.BuilderCommand((obj) => ShowWords(obj as IGroup));
            TranslationDirectionChangedCommand = new Util.BuilderCommand(ActionsSingleton<TranslationDirectionChangeAction>.Instance.Action);
            AllWordsCommand = new Util.BuilderCommand(ActionsSingleton<AllWordsChangeAction>.Instance.Action);
            SelectionChangedCommand = new Util.BuilderCommand(SelectionChanged);
        }

        #region Commands

        private void EditGroup(IGroup group)
        {
            if (group == null)
            {
                Console.WriteLine("The parameter is equale null. Cannot switch to other state");
                return;
            }
            Switcher.Switch(Switcher.State.Builder, group);
        }

        private void ShowWords(IGroup group)
        {
            if (group == null)
            {
                Console.WriteLine("The parameter is equale null. Cannot switch to other state");
                return;
            }
            Switcher.Switch(Switcher.State.Words, group);
        }

        private void BackAction()
        {
            Back();
            Switcher.Back();
        }

        private void SelectionChanged(object obj)
        {
            if (obj == null)
                return;
            IList lList = (obj as IList);
            if (lList == null)
                return;
            SelectedItems.Clear();
            SelectedItems.AddRange(lList.Cast<IGroup>());
            RefreshInfo();
        }

        private void StartLesson(LessonType type)
        {
            try
            {
                Lesson lesson = LessonFactory.CreateLesson(type);
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
                };
                lesson.LessonSettings = lessonSettings;

                ILessonWordsCreator creator = LessonWordCreatorFactory.Create(type);
                creator.AllWords = UserManagerSingleton.Instence.User.AllWords;
                creator.Count = 30;
                creator.Groups = SelectedItems;
                lesson.InitLesson(creator.GetWords());
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
                Switcher.Switch(GetStateFromLessonType(type), lesson);
            }
            catch (Exception lException)
            {
                LoggerSingleton.LogError("{0} - {1} - {2}", "Blad w startowaniu lekcji", type.ToString(), lException.Message);
            }
        }

        #endregion

        public override void InitViewModel(object parameter = null)
        {
            User = UserManagerSingleton.Instence.User;
            if (Database.Groups.Count > 0)
            {
                SelectedItem = Database.Groups[0];
            }
        }

        public override void Back()
        {

        }

        private void RefreshInfo()
        {
            if (Database.Groups.Count == 0)
            {
                return;
            }
            List<double> lDrawersCount = new List<double>();
            LanguageType lang1 = SelectedItem.Language1;
            LanguageType lang2 = SelectedItem.Language2;
            DateTime lLastRepeat = DateTime.MinValue;
            for (int i = 0; i < 5; i++)
                lDrawersCount.Add(0);
            if (SelectedItems == null)
                return;
            foreach (Group item in SelectedItems)
            {
                lang1 = lang1 != item.Language1 && lang1 != LanguageType.Default ? LanguageType.Default : lang1;
                lang2 = lang2 != item.Language2 && lang2 != LanguageType.Default ? LanguageType.Default : lang2;
                foreach (Word lWord in item.Words)
                {
                    lDrawersCount[lWord.Drawer]++;
                }
                DateTime itemDateTime = item.Results.Max(x => x.DateTime);
                if (itemDateTime > lLastRepeat)
                {
                    lLastRepeat = itemDateTime;
                }
            }
            string groupName = SelectedItems.Count == 1 ? SelectedItems.First().Name : " ";
            int lWordsCount = SelectedItems.Sum(x => x.Words.Count);
            int lVisibilitiesCount = SelectedItems.Sum(x => x.Words.Count(y => y.IsVisible));
            int lRepeatsCount = SelectedItems.Sum(x => x.Results.Count(y => y.TranslationDirection == UserManagerSingleton.Instence.User.TranslationDirection));
            MaxValue = lDrawersCount.Sum();
            Values = new ObservableCollection<double>(lDrawersCount);
            GroupInfo.SetValue(groupName, lWordsCount, lRepeatsCount, lLastRepeat, lVisibilitiesCount, LanguageFactory.GetLanguage(lang1).Flag, LanguageFactory.GetLanguage(lang2).Flag);
        }

        public override void Loaded()
        {
        }

        public override void Unloaded()
        {
        }

        private Switcher.State GetStateFromLessonType(LessonType type)
        {
            Switcher.State stateToStart;
            switch (type)
            {
                case LessonType.FiszkiLesson:
                    {
                        stateToStart = Switcher.State.TeachFiszki;
                    }
                    break;
                case LessonType.TypingLesson:
                    {
                        stateToStart = Switcher.State.TeachTyping;
                    }
                    break;
                case LessonType.IntensiveLesson:
                    {
                        stateToStart = Switcher.State.TeachTyping;
                    }
                    break;
                case LessonType.RandomLesson:
                    {
                        stateToStart = Switcher.State.TeachTyping;
                    }
                    break;
                case LessonType.BestLesson:
                    {
                        stateToStart = Switcher.State.TeachTyping;
                    }
                    break;
                case LessonType.AllToTeach:
                    {
                        stateToStart = Switcher.State.TeachTyping;
                    }
                    break;
                default:
                    {
                        throw new IndexOutOfRangeException($"There is not possible to choose Switcher.State from type {type}");
                    }
            }
            return stateToStart;
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

