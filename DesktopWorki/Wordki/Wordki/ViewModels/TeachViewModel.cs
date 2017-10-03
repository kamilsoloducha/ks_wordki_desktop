using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Repository.Models.Enums;
using Wordki.Helpers;
using Wordki.Models;
using Wordki.Models.Lesson;
using Wordki.Models.RemoteDatabase;
using Wordki.Views.Dialogs;
using Wordki.Helpers.Command;

namespace Wordki.ViewModels
{
    public class TeachViewModel : ViewModelBase, ITimerListener
    {
        private LessonState _lessonState;

        #region Properies
        private Lesson Lesson { get; set; }
        private int TimeOutTicks { get; set; }
        private bool TimeOutChecking { get; set; }
        public Settings Settings { get; set; }

        public LessonState State
        {
            get { return _lessonState; }
            set
            {
                if (_lessonState == value) return;
                _lessonState = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<InputBinding> _shortKeys;
        public ObservableCollection<InputBinding> ShortKeys
        {
            get { return _shortKeys; }
            set
            {
                if (_shortKeys == value) return;
                _shortKeys = value;
                OnPropertyChanged();
            }
        }

        private int _selectionStart;
        public int SelectionStart
        {
            get { return _selectionStart; }
            set
            {
                if (_selectionStart == value) return;
                _selectionStart = value;
                OnPropertyChanged();
            }
        }

        private int _selectionLength;
        public int SelectionLength
        {
            get { return _selectionLength; }
            set
            {
                if (_selectionLength == value) return;
                _selectionLength = value;
                OnPropertyChanged();
            }
        }

        private string _timer;
        public string Timer
        {
            get { return _timer; }
            set
            {
                if (_timer == value) return;
                _timer = value;
                OnPropertyChanged();
            }
        }
        public ObservableDictionary<int, Visibility> VisibilityDictionary { get; set; }
        public BuilderCommand UnknownCommand { get; set; }
        public BuilderCommand CheckCommand { get; set; }
        public BuilderCommand KnownCommand { get; set; }
        public BuilderCommand BackCommand { get; set; }
        public BuilderCommand CorrectCommand { get; set; }
        public BuilderCommand OnEnterClickCommand { get; set; }
        public static BuilderCommand StartLessonCommand { get; set; }
        public static BuilderCommand PauseCommand { get; set; }
        private LessonStateFactory StateFactory { get; set; }
        #endregion

        public TeachViewModel()
        {
            ActivateCommands();

            VisibilityDictionary = new ObservableDictionary<int, Visibility> {
        {0, Visibility.Collapsed},
        {1, Visibility.Collapsed},
      };
            ShortKeys = new ObservableCollection<InputBinding>
      {
        new KeyBinding {
          Command = new BuilderCommand(ShortKey),
          CommandParameter = 'ś',
          Key = Key.S,
          Modifiers = ModifierKeys.Alt
        }
      };
        }

        private void ShortKey(object obj)
        {
            char character = (char)obj;
            State.Translation += character;
        }

        public override void InitViewModel()
        {
            Settings = Settings.GetSettings();
            StateFactory = new LessonStateFactory();

            Lesson lLesson = (Lesson)PackageStore.Get(0);
            if (lLesson == null)
                return;
            lLesson.Timer.TimerListeners.Add(this);
            Timer = "";
            Lesson = lLesson;

            State = StateFactory.GetState(Lesson, LessonStateEnum.BeforeStart);
            State.RefreshView();

            VisibilityDictionary.Set(0, Visibility.Collapsed);
            VisibilityDictionary.Set(1, Visibility.Collapsed);
            OnTimerTick(0);
        }

        public override void Back() { }


        #region Commands

        private void ActivateCommands()
        {
            UnknownCommand = new BuilderCommand(Unknown);
            CheckCommand = new BuilderCommand(Check);
            KnownCommand = new BuilderCommand(Known);
            BackCommand = new BuilderCommand(Back);
            CorrectCommand = new BuilderCommand(Correct);
            OnEnterClickCommand = new BuilderCommand(OnEnterClick);
            StartLessonCommand = new BuilderCommand(StartLesson);
            PauseCommand = new BuilderCommand(Pause);
        }

        private void Pause(object obj)
        {
            LessonStateEnum lStateEnum = State.StateEnum;
            InfoDialog lDialog = new InfoDialog();
            lDialog.ButtonCommand = new BuilderCommand(o =>
            {
                lDialog.Close();
                State = StateFactory.GetState(Lesson, lStateEnum);
                if (State != null)
                {
                    State.RefreshView();
                }
            });
            lDialog.ButtonLabel = "Koniec";
            lDialog.Message = "Przerwa";
            lDialog.DialogTitle = "Uwaga";
            using (LessonState lState = StateFactory.GetState(Lesson, LessonStateEnum.Pause))
            {
                State = lState;
                State.RefreshView();
                lDialog.ShowDialog();
            }
        }

        private void StartLesson(object obj)
        {
            try
            {
                Lesson.Timer.StartTimer();
                Lesson.NextWord();
                State = StateFactory.GetState(Lesson, LessonStateEnum.NextWord);
                State.RefreshView();
                Result lResult = Lesson.ResultList.FirstOrDefault();
                if (lResult == null)
                {
                    Logger.LogError("{0} - {1}", "TeachViewModel.StartLesson", "lResult == null");
                    return;
                }
                switch (lResult.LessonType)
                {
                    case LessonType.TypingLesson:
                    case LessonType.IntensiveLesson:
                    case LessonType.RandomLesson:
                        VisibilityDictionary[0] = Visibility.Visible;
                        break;
                    case LessonType.FiszkiLesson:
                        VisibilityDictionary[1] = Visibility.Visible;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                TimeOutChecking = true;
            }
            catch (Exception lException)
            {
                Logger.LogError("{0} - {1}", "TeachViewModel.StartLesson", lException.Message);
            }
        }

        private async void Correct(object obj)
        {
            if (Lesson == null || Lesson.SelectedWord == null)
                return;
            LessonStateEnum lLastState = State.StateEnum;
            using (LessonState lPauseState = StateFactory.GetState(Lesson, LessonStateEnum.Pause))
            {
                State = lPauseState;
                State.RefreshView();
                CorrectWordDialog lDialog = new CorrectWordDialog(Lesson.SelectedWord);
                lDialog.ShowDialog();
                bool? lResult = lDialog.DialogResult;
                if (lResult.HasValue && lResult.Value)
                {//delete
                    await Database.GetDatabase().DeleteWordAsync(Lesson.SelectedWord.GroupId, Lesson.SelectedWord);
                    //usunięcie śladu po słowie w wynikach
                    if (Lesson.Counter > Lesson.BeginWordsList.Count)
                    {
                        Lesson.ResultList.First(x => x.GroupId == Lesson.SelectedWord.GroupId).Wrong--;
                    }
                    Lesson.BeginWordsList.Remove(Lesson.SelectedWord);
                    Lesson.NextWord();
                    CheckNextState();
                }
                else if (lResult.HasValue && !lResult.Value)
                {//correct
                    State = StateFactory.GetState(Lesson, lLastState);
                    State.RefreshView();
                }
                else
                {//cancel
                    State = StateFactory.GetState(Lesson, lLastState);
                    State.RefreshView();
                }
            }
        }

        private void Back(object obj)
        {
            LessonStateEnum lLastState = State.StateEnum;
            YesNoDialog lDialog = new YesNoDialog();
            lDialog.DialogTitle = "Uwaga";
            lDialog.Message = "Czy na pewno chcesz opuścić lekcje?";
            lDialog.PositiveLabel = "Tak";
            lDialog.NegativeLabel = "Nie";
            lDialog.PositiveCommand = new BuilderCommand(delegate
            {
                if (Lesson != null)
                {
                    try
                    {
                        ObjectSerializer.WriteToBinaryFile("lesson", Lesson);
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("Błąd serializowania lekcji - {0}", e.Message);
                    }
                    Lesson.FinishLesson();
                    Lesson.Timer.StopTimer();
                }
                lDialog.Close();
                Switcher.GetSwitcher().Back(true);
            });
            lDialog.NegativeCommand = new BuilderCommand(delegate
            {
                State = StateFactory.GetState(Lesson, lLastState);
                if (State != null)
                {
                    State.RefreshView();
                }
                lDialog.Close();
            });
            State = StateFactory.GetState(Lesson, LessonStateEnum.Pause);
            State.RefreshView();
            lDialog.ShowDialog();
        }

        private void OnEnterClick(object obj)
        {
            if (Lesson.IsChecked)
            {
                if (Lesson.IsCorrect)
                {
                    Known(null);
                }
                else
                {
                    Unknown(null);
                }
            }
            else
            {
                Check(null);
            }
        }

        private void Known(object obj)
        {
            Lesson.Known();
            CheckNextState();
            TimeOutChecking = true;
        }

        private void Check(object obj)
        {
            Lesson.Translation = State.Translation;
            Lesson.Check();

            if (Lesson.IsCorrect)
            {
                State = StateFactory.GetState(Lesson, LessonStateEnum.Correct);
                State.RefreshView();
            }
            else
            {
                State = StateFactory.GetState(Lesson, LessonStateEnum.Wrong);
                State.RefreshView();
            }
            TimeOutChecking = false;
        }

        private void Unknown(object obj)
        {
            Lesson.Unknown();
            CheckNextState();
            TimeOutChecking = true;
        }

        #endregion

        public void OnTimerTick(int pTicks)
        {
            if (pTicks < 3600)
            {
                int lMinutes = pTicks / 60;
                int lSecounds = pTicks - lMinutes * 60;
                Timer = String.Format("{0}m : {1}s", lMinutes, lSecounds);
            }
            else
            {
                int lHours = pTicks / 3600;
                int lMinutes = (pTicks - lHours * 3600) / 60;
                Timer = String.Format("{0}g : {1}m", lHours, lMinutes);
            }
            //sprawdzanie TimeOut
            CheckTimeOut();
        }

        private void CheckTimeOut()
        {
            if (UserManager.GetInstance().User.Timeout == 0)
                return;
            if (!TimeOutChecking)
            {
                return;
            }
            TimeOutTicks++;
            if (TimeOutTicks < UserManager.GetInstance().User.Timeout)
                return;
            TimeOutTicks = 0;
            Check(null);
        }

        private void CheckNextState()
        {
            if (Lesson.SelectedWord != null)
            {
                State = StateFactory.GetState(Lesson, LessonStateEnum.NextWord);
            }
            else
            {
                State = StateFactory.GetState(Lesson, LessonStateEnum.AfterEnd);
            }
            State.RefreshView();
        }
        //==========================================================
        #region StateDesignPattern

        public abstract class LessonState : INotifyPropertyChanged, IDisposable
        {
            protected TranslationDirection _translationDirection;
            protected Lesson Lesson { get; set; }
            public LessonStateEnum StateEnum { get; set; }

            #region Properties
            private string _groupName;
            public string GroupName
            {
                get { return _groupName; }
                set
                {
                    if (_groupName == value) return;
                    _groupName = value;
                    OnPropertyChanged();
                }
            }

            private string _translation;
            public string Translation
            {
                get { return _translation; }
                set
                {
                    if (_translation == value) return;
                    _translation = value;
                    OnPropertyChanged();
                }
            }

            private string _label;
            public string Label
            {
                get { return _label; }
                set
                {
                    if (_label == value) return;
                    _label = value;
                    OnPropertyChanged();
                }
            }

            private string _commentLanguage;
            public string CommentLanguage
            {
                get { return _commentLanguage; }
                set
                {
                    if (_commentLanguage == value) return;
                    _commentLanguage = value;
                    OnPropertyChanged();
                }
            }

            private string _commentTranslation;
            public string CommentTranslation
            {
                get { return _commentTranslation; }
                set
                {
                    if (_commentTranslation == value) return;
                    _commentTranslation = value;
                    OnPropertyChanged();
                }
            }

            private bool _correctButtonEnabled;
            public bool CorrectButtonEnabled
            {
                get { return _correctButtonEnabled; }
                set
                {
                    if (_correctButtonEnabled == value) return;
                    _correctButtonEnabled = value;
                    OnPropertyChanged();
                }
            }

            private bool _checkButtonEnabled;
            public bool CheckButtonEnabled
            {
                get { return _checkButtonEnabled; }
                set
                {
                    if (_checkButtonEnabled == value) return;
                    _checkButtonEnabled = value;
                    OnPropertyChanged();
                }
            }

            private bool _wrongButtonEnabled;
            public bool WrongButtonEnabled
            {
                get { return _wrongButtonEnabled; }
                set
                {
                    if (_wrongButtonEnabled == value) return;
                    _wrongButtonEnabled = value;
                    OnPropertyChanged();
                }
            }

            private bool _translationBoxEnabled;
            public bool TranslationBoxEnabled
            {
                get { return _translationBoxEnabled; }
                set
                {
                    if (_translationBoxEnabled == value) return;
                    _translationBoxEnabled = value;
                    OnPropertyChanged();
                }
            }

            private bool _translationIsFocused;
            public bool TranslationIsFocused
            {
                get { return _translationIsFocused; }
                set
                {
                    if (_translationIsFocused == value) return;
                    _translationIsFocused = value;
                    OnPropertyChanged();
                }
            }

            private bool _wrongButtonIsFocused;
            public bool WrongButtonIsFocused
            {
                get { return _wrongButtonIsFocused; }
                set
                {
                    if (_wrongButtonIsFocused == value) return;
                    _wrongButtonIsFocused = value;
                    OnPropertyChanged();
                }
            }

            private bool _correctButtonIsFocused;
            public bool CorrectButtonIsFocused
            {
                get { return _correctButtonIsFocused; }
                set
                {
                    if (_correctButtonIsFocused == value) return;
                    _correctButtonIsFocused = value;
                    OnPropertyChanged();
                }
            }

            private Brush _translationColor;
            public Brush TranslationColor
            {
                get { return _translationColor; }
                set
                {
                    if (_translationColor != null && _translationColor.Equals(value)) return;
                    _translationColor = value;
                    OnPropertyChanged();
                }
            }

            private ObservableCollection<double> _drawerValues;
            public ObservableCollection<double> DrawerValues
            {
                get { return _drawerValues; }
                set
                {
                    if (_drawerValues == value) return;
                    _drawerValues = value;
                    OnPropertyChanged();
                }
            }

            private int _maxValue;
            public int MaxValue
            {
                get { return _maxValue; }
                set
                {
                    if (_maxValue == value) return;
                    _maxValue = value;
                    OnPropertyChanged();
                }
            }

            private int _maxResult;
            public int MaxResult
            {
                get { return _maxResult; }
                set
                {
                    if (_maxResult == value) return;
                    _maxResult = value;
                    OnPropertyChanged();
                }
            }

            private ObservableCollection<double> _progress;
            public ObservableCollection<double> Progress
            {
                get { return _progress; }
                set
                {
                    if (_progress == value) return;
                    _progress = value;
                    OnPropertyChanged();
                }
            }

            private ObservableCollection<double> _results;
            public ObservableCollection<double> Results
            {
                get { return _results; }
                set
                {
                    if (_results == value) return;
                    _results = value;
                    OnPropertyChanged();
                }
            }

            private ObservableCollection<double> _counter;
            public ObservableCollection<double> Counter
            {
                get { return _counter; }
                set
                {
                    if (_counter == value) return;
                    _counter = value;
                    OnPropertyChanged();
                }
            }

            private int _selectedDrawer;
            public int SelectedDrawer
            {
                get { return _selectedDrawer; }
                set
                {
                    if (_selectedDrawer == value) return;
                    _selectedDrawer = value;
                    OnPropertyChanged();
                }
            }

            private string _startStopButtonContent;
            public string StartStopButtonContent
            {
                get { return _startStopButtonContent; }
                set
                {
                    if (_startStopButtonContent == value) return;
                    _startStopButtonContent = value;
                    OnPropertyChanged();
                }
            }

            private BuilderCommand _startStopButtonCommand;
            public BuilderCommand StartStopButtonCommand
            {
                get { return _startStopButtonCommand; }
                set
                {
                    if (_startStopButtonCommand == value) return;
                    _startStopButtonCommand = value;
                    OnPropertyChanged();
                }
            }

            #endregion

            protected LessonState(Lesson pLesson)
              : this(pLesson, null) { }

            protected LessonState(Lesson pLesson, LessonState pLastState)
            {
                Lesson = pLesson;
                _translationDirection = UserManager.GetInstance().User.TranslationDirection;
                if (pLastState != null)
                {
                    _drawerValues = pLastState._drawerValues;
                    _results = pLastState._results;
                    _counter = pLastState._counter;
                    _selectedDrawer = pLastState._selectedDrawer;
                    _progress = pLastState._progress;
                }
                else
                {
                    _drawerValues = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
                    _results = new ObservableCollection<double> { 0, 0, 0 };
                    _progress = new ObservableCollection<double> { 0 };
                    _counter = new ObservableCollection<double> { 0 };
                }
            }

            protected abstract void RefreshGroupName();
            protected abstract void RefreshTranslation();
            protected abstract void RefreshLabel();
            protected abstract void RefreshComments();
            protected abstract void RefreshCorrectButtonEnabled();
            protected abstract void RefreshCheckButtonEnabled();
            protected abstract void RefreshWrongButtonEnabled();
            protected abstract void RefreshTranslationColor();
            protected abstract void RefreshTranslationBoxEnabled();
            protected abstract void RefreshFocused();
            protected abstract void RefreshDrawerLight();
            protected abstract void RefreshStartStopButton();

            protected void RefreshDrawersValues()
            {
                if (Lesson == null) return;
                int[] values = Lesson.GetDrawerValues();
                for (int i = 0; i < values.Length; i++)
                {
                    DrawerValues[i] = values[i];
                }
                MaxValue = Lesson.BeginWordsList.Count;
            }

            protected void RefreshProgress()
            {
                if (Lesson == null)
                    return;
                if (Lesson.BeginWordsList.Count != 0)
                    Progress[0] = Lesson.GetProgress();
            }

            protected virtual void RefreshResult()
            {
                if (Lesson.Counter > Lesson.BeginWordsList.Count)
                {
                    Counter[0] = Lesson.BeginWordsList.Count;
                }
                else
                {
                    Counter[0] = Lesson.Counter;
                }
                Results[2] = Lesson.GetCorrect();
                Results[1] = Lesson.GetAccepted();
                Results[0] = Lesson.GetWrong();
                MaxResult = (int)Results.Sum();
            }

            public virtual void RefreshView()
            {
                RefreshGroupName();
                RefreshTranslation();
                RefreshLabel();
                RefreshComments();
                RefreshCorrectButtonEnabled();
                RefreshCheckButtonEnabled();
                RefreshWrongButtonEnabled();
                RefreshTranslationColor();
                RefreshTranslationBoxEnabled();
                RefreshFocused();
                RefreshDrawersValues();
                RefreshDrawerLight();
                RefreshProgress();
                RefreshResult();
                RefreshStartStopButton();
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string pPropertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
                }
            }

            protected string GetNewLabel()
            {
                switch (_translationDirection)
                {
                    case TranslationDirection.FromFirst:
                        return Lesson.SelectedWord.Language1;
                    case TranslationDirection.FromSecond:
                        return Lesson.SelectedWord.Language2;
                }
                return "%ERROR%";
            }

            protected string GetTransaltion()
            {
                switch (_translationDirection)
                {
                    case TranslationDirection.FromFirst:
                        return Lesson.SelectedWord.Language2;
                    case TranslationDirection.FromSecond:
                        return Lesson.SelectedWord.Language1;
                }
                return "%ERROR%";
            }

            protected string GetComment()
            {
                switch (_translationDirection)
                {
                    case TranslationDirection.FromFirst:
                        return Lesson.SelectedWord.Language1Comment;
                    case TranslationDirection.FromSecond:
                        return Lesson.SelectedWord.Language2Comment;
                }
                return "%ERROR%";
            }

            protected string GetCommentTranslation()
            {
                switch (_translationDirection)
                {
                    case TranslationDirection.FromFirst:
                        return Lesson.SelectedWord.Language2Comment;
                    case TranslationDirection.FromSecond:
                        return Lesson.SelectedWord.Language1Comment;
                }
                return "%ERROR%";
            }

            public void Dispose()
            {
            }
        }

        private class LessonStateFactory
        {

            private readonly Dictionary<LessonStateEnum, LessonState> LessonStates = new Dictionary<LessonStateEnum, LessonState>();
            private LessonStateEnum _selectedState = LessonStateEnum.None;

            public LessonState GetState(Lesson pLesson, LessonStateEnum pStateEnum)
            {
                LessonState lastState;
                LessonStates.TryGetValue(_selectedState, out lastState);
                _selectedState = pStateEnum;

                if (!LessonStates.ContainsKey(pStateEnum))
                {
                    LessonState lState;
                    switch (pStateEnum)
                    {
                        case LessonStateEnum.BeforeStart:
                            lState = new BeforeStartState(pLesson, lastState);
                            break;
                        case LessonStateEnum.AfterEnd:
                            lState = new AfterEndState(pLesson, lastState);
                            break;
                        case LessonStateEnum.NextWord:
                            lState = new NextWordState(pLesson, lastState);
                            break;
                        case LessonStateEnum.Correct:
                            lState = new CorrectState(pLesson, lastState);
                            break;
                        case LessonStateEnum.Wrong:
                            lState = new WrongState(pLesson, lastState);
                            break;
                        case LessonStateEnum.Pause:
                            lState = new PauseState(pLesson, lastState);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("pStateEnum");
                    }
                    LessonStates.Add(pStateEnum, lState);
                    return lState;
                }
                return LessonStates[pStateEnum];
            }
        }

        private class BeforeStartState : LessonState
        {
            public BeforeStartState(Lesson pLesson, LessonState pLastState)
              : base(pLesson, pLastState)
            {
                StateEnum = LessonStateEnum.BeforeStart;
            }

            protected override void RefreshGroupName()
            {
            }

            protected override void RefreshTranslation()
            {
                Translation = "";
            }

            protected override void RefreshLabel()
            {
                Label = "";
            }

            protected override void RefreshComments()
            {
                CommentTranslation = "";
                CommentLanguage = "";
            }

            protected override void RefreshCorrectButtonEnabled() { }

            protected override void RefreshCheckButtonEnabled() { }

            protected override void RefreshWrongButtonEnabled() { }

            protected override void RefreshTranslationBoxEnabled() { }

            protected override void RefreshTranslationColor() { }

            protected override void RefreshFocused() { }

            protected override void RefreshDrawerLight() { }

            protected override void RefreshStartStopButton()
            {
                StartStopButtonContent = "Start";
                StartStopButtonCommand = StartLessonCommand;
            }

            protected override void RefreshResult()
            {
                base.RefreshResult();
                Counter[0] = 0;
            }
        }

        private class NextWordState : LessonState
        {

            public NextWordState(Lesson pLesson, LessonState pLastState)
              : base(pLesson, pLastState)
            {
                StateEnum = LessonStateEnum.NextWord;
            }

            protected override void RefreshGroupName()
            {
                GroupName = Database.GetDatabase().GetGroupById(Lesson.SelectedWord.GroupId).Name;
            }

            protected override void RefreshTranslation()
            {
                Translation = "";
            }

            protected override void RefreshLabel()
            {
                Label = GetNewLabel();
            }

            protected override void RefreshComments()
            {
                CommentTranslation = "";
                CommentLanguage = Settings.GetSettings().ShowCommentsBefore ? GetComment() : "";
            }

            protected override void RefreshCorrectButtonEnabled() { }

            protected override void RefreshCheckButtonEnabled()
            {
                CheckButtonEnabled = true;
            }

            protected override void RefreshWrongButtonEnabled() { }

            protected override void RefreshTranslationBoxEnabled()
            {
                TranslationBoxEnabled = true;
            }

            protected override void RefreshTranslationColor()
            {
                TranslationColor = Application.Current.Resources["UsedNormalFrontBrush"] as Brush;
            }

            protected override void RefreshFocused()
            {
                TranslationIsFocused = true;
            }

            protected override void RefreshDrawerLight()
            {
                SelectedDrawer = Lesson.CurrentDrawer;
            }

            protected override void RefreshStartStopButton()
            {
                StartStopButtonContent = "Stop";
                StartStopButtonCommand = PauseCommand;
            }
        }

        private class CorrectState : LessonState
        {

            public CorrectState(Lesson pLesson, LessonState pLastState)
              : base(pLesson, pLastState)
            {
                StateEnum = LessonStateEnum.Correct;
            }

            protected override void RefreshGroupName()
            {
                GroupName = Database.GetDatabase().GetGroupById(Lesson.SelectedWord.GroupId).Name;//todo trzeba to przyspieszyc bo nie oplaca sie szukac cały czas tego
            }

            protected override void RefreshTranslation()
            {
                Translation = GetTransaltion();
            }

            protected override void RefreshLabel()
            {
                Label = GetNewLabel();
            }

            protected override void RefreshComments()
            {
                CommentTranslation = GetCommentTranslation();
                CommentLanguage = GetComment();
            }

            protected override void RefreshCorrectButtonEnabled()
            {
                CorrectButtonEnabled = true;
            }

            protected override void RefreshCheckButtonEnabled() { }

            protected override void RefreshWrongButtonEnabled()
            {
                WrongButtonEnabled = true;
            }

            protected override void RefreshTranslationBoxEnabled() { }

            protected override void RefreshTranslationColor()
            {
                TranslationColor = Brushes.Green;
            }

            protected override void RefreshFocused()
            {
                CorrectButtonIsFocused = true;
            }

            protected override void RefreshDrawerLight()
            {
                SelectedDrawer = Lesson.CurrentDrawer;
            }
            protected override void RefreshStartStopButton()
            {
                StartStopButtonContent = "Stop";
                StartStopButtonCommand = PauseCommand;
            }
        }

        private class WrongState : LessonState
        {

            public WrongState(Lesson pLesson, LessonState pLastState)
              : base(pLesson, pLastState)
            {
                StateEnum = LessonStateEnum.Wrong;
            }

            protected override void RefreshGroupName()
            {
                GroupName = Database.GetDatabase().GetGroupById(Lesson.SelectedWord.GroupId).Name;
            }

            protected override void RefreshTranslation()
            {
                switch (_translationDirection)
                {
                    case TranslationDirection.FromFirst:
                        Translation = string.Format("{0} / {1}", Lesson.Translation, Lesson.SelectedWord.Language2);
                        break;
                    case TranslationDirection.FromSecond:
                        Translation = string.Format("{0} / {1}", Lesson.Translation, Lesson.SelectedWord.Language1);
                        break;
                }
            }

            protected override void RefreshLabel()
            {
                Label = GetNewLabel();
            }

            protected override void RefreshComments()
            {
                CommentTranslation = GetCommentTranslation();
                CommentLanguage = GetComment();
            }

            protected override void RefreshCorrectButtonEnabled()
            {
                CorrectButtonEnabled = true;
            }

            protected override void RefreshCheckButtonEnabled() { }

            protected override void RefreshWrongButtonEnabled()
            {
                WrongButtonEnabled = true;
            }

            protected override void RefreshTranslationBoxEnabled() { }

            protected override void RefreshTranslationColor()
            {
                TranslationColor = Brushes.Red;
            }

            protected override void RefreshFocused()
            {
                WrongButtonIsFocused = true;
            }

            protected override void RefreshDrawerLight()
            {
                SelectedDrawer = Lesson.CurrentDrawer;
            }
            protected override void RefreshStartStopButton()
            {
                StartStopButtonContent = "Stop";
                StartStopButtonCommand = PauseCommand;
            }
        }

        private class AfterEndState : LessonState
        {

            public AfterEndState(Lesson pLesson, LessonState pLastState)
              : base(pLesson, pLastState)
            {
                StateEnum = LessonStateEnum.AfterEnd;
            }

            protected override void RefreshGroupName() { }

            protected override void RefreshTranslation() { }

            protected override void RefreshLabel() { }

            protected override void RefreshComments() { }

            protected override void RefreshCorrectButtonEnabled() { }

            protected override void RefreshCheckButtonEnabled() { }

            protected override void RefreshWrongButtonEnabled() { }

            protected override void RefreshTranslationBoxEnabled() { }

            protected override void RefreshTranslationColor() { }

            public override void RefreshView()
            {
                base.RefreshView();
                Lesson.FinishLesson();
                Lesson.Timer.StopTimer();
                IList<Group> lGroupList = Lesson.ResultList.Select(lResult => Database.GetDatabase().GetGroupById(lResult.GroupId)).ToList();
                CommandQueue<Helpers.Command.ICommand> lQueue = RemoteDatabaseBase.GetRemoteDatabase(UserManager.GetInstance().User).GetUploadQueue();
                lQueue.MainQueue.AddFirst(new SimpleCommandAsync(async () =>
                {
                    IDatabase lDatabase = Models.Database.GetDatabase();
                    foreach (Result lItem in Lesson.ResultList)
                    {
                        await Database.GetDatabase().AddResultAsync(lItem);
                    }
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LessonResultDialog lDialog = new LessonResultDialog(lGroupList);
                        lDialog.ButtonCommand = new BuilderCommand(delegate
                        {
                            lDialog.Close();
                            Switcher.GetSwitcher().Back(true);
                        });
                        lDialog.ShowDialog();
                    });
                    IDatabase database = Models.Database.GetDatabase();
                    foreach (Word lItem in Lesson.BeginWordsList)
                    {
                        Word word = database.GetGroupById(lItem.GroupId).WordsList.FirstOrDefault(x => x.Id == lItem.Id);
                        if (word == null)
                        {
                            continue;
                        }
                        word.Drawer = lItem.Drawer;
                        await lDatabase.UpdateWordAsync(word);
                    }
                    return true;
                }));
                lQueue.OnQueueComplete += success =>
                {
                    if (success)
                    {
                        Database.GetDatabase().RefreshDatabase();
                    }
                };
                lQueue.CreateDialog = false;
                lQueue.Execute();
            }

            protected override void RefreshFocused() { }
            protected override void RefreshDrawerLight()
            {
                SelectedDrawer = -1;
            }
            protected override void RefreshStartStopButton()
            {
                StartStopButtonContent = "Stop";
                StartStopButtonCommand = PauseCommand;
            }
        }

        private class PauseState : LessonState, IDisposable
        {
            public PauseState(Lesson pLesson, LessonState pLastState)
              : base(pLesson, pLastState)
            {
                StateEnum = LessonStateEnum.Pause;
            }

            protected override void RefreshGroupName() { }

            protected override void RefreshTranslation() { }

            protected override void RefreshLabel() { }

            protected override void RefreshComments() { }

            protected override void RefreshCorrectButtonEnabled() { }

            protected override void RefreshCheckButtonEnabled() { }

            protected override void RefreshWrongButtonEnabled() { }

            protected override void RefreshTranslationBoxEnabled() { }

            protected override void RefreshTranslationColor() { }

            protected override void RefreshFocused() { }

            protected override void RefreshDrawerLight() { }

            protected override void RefreshStartStopButton()
            {
                StartStopButtonContent = "Stop";
                StartStopButtonCommand = PauseCommand;
            }

            public override void RefreshView()
            {
                base.RefreshView();
                Lesson.Timer.Pause();
            }

            public new void Dispose()
            {
                Lesson.Timer.Resume();
            }
        }

        public enum LessonStateEnum
        {
            None,
            BeforeStart,
            AfterEnd,
            NextWord,
            Correct,
            Wrong,
            Pause
        }

        #endregion



    }
}