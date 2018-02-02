using System;
using System.Linq;
using System.Windows.Input;
using Wordki.Helpers;
using Wordki.Models;
using Wordki.Models.Lesson;
using WordkiModel;
using Wordki.Database;
using Wordki.InteractionProvider;
using Wordki.ViewModels.Dialogs;
using Wordki.ViewModels.LessonStates;
using Wordki.Commands;
using Oazachaosu.Core.Common;
using NLog;

namespace Wordki.ViewModels
{
    public abstract partial class TeachViewModelBase : ViewModelBase, Util.Timer.ITimerListener
    {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Properies
        public static Switcher switcher;
        protected static Lesson Lesson { get; set; }
        protected int HintLetters { get; set; }
        public Settings Settings { get; set; }

        private LessonStateBase _lessonState;
        public LessonStateBase State
        {
            get { return _lessonState; }
            set
            {
                if (_lessonState == value) return;
                _lessonState = value;
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

        private bool _cursonOnEnd;
        public bool CursorOnEnd
        {
            get { return _cursonOnEnd; }
            set
            {
                if (_cursonOnEnd == value) return;
                _cursonOnEnd = value;
                OnPropertyChanged();
            }
        }

        private bool _enableTextBox;
        public bool EnableTextBox
        {
            get { return _enableTextBox; }
            set
            {
                if (_enableTextBox == value)
                {
                    return;
                }
                _enableTextBox = value;
                OnPropertyChanged();
            }
        }

        public ICommand UnknownCommand { get; }
        public ICommand CheckCommand { get; }
        public ICommand KnownCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand CorrectCommand { get; }
        public ICommand OnEnterClickCommand { get; }
        public ICommand HintCommand { get; }
        public ICommand CheckUncheckCommand { get; }
        public static ICommand StartLessonCommand { get; set; }
        public static ICommand PauseCommand { get; set; }
        protected LessonStateFactoryBase StateFactory { get; set; }
        #endregion

        public TeachViewModelBase()
        {
            Settings = Settings.GetSettings();
            HintLetters = 0;

            CheckUncheckCommand = new Util.BuilderCommand((obj) => ActionsSingleton<CheckUncheckAction>.Instance.Action(obj as IWord));
            UnknownCommand = new Util.BuilderCommand(Unknown);
            CheckCommand = new Util.BuilderCommand(Check);
            KnownCommand = new Util.BuilderCommand(Known);
            BackCommand = new Util.BuilderCommand(BackAction);
            CorrectCommand = new Util.BuilderCommand(Correct);
            OnEnterClickCommand = new Util.BuilderCommand(OnEnterClick);
            HintCommand = new Util.BuilderCommand(Hint);
        }

        public override void InitViewModel(object parameter = null)
        {
            switcher = Switcher;
            Lesson = parameter as Lesson;
            if (Lesson == null)
                return;
            Lesson.Timer.TimerListeners.Add(this);
            Timer = "";
            OnTimerTick(0);
            StartLessonCommand = new Util.BuilderCommand(StartLesson);
            PauseCommand = new Util.BuilderCommand(Pause);
        }

        public override void Back()
        {
            Lesson.Timer.TimerListeners.Remove(this);
        }

        protected abstract void Check();

        #region Commands

        private void Hint(object obj)
        {
            CursorOnEnd = true;
            HintLetters++;
            if (Lesson.SelectedWord == null)
            {
                return;
            }
            if (!Lesson.Timer.IsRunning)
            {
                return;
            }
            string translation = UserManagerSingleton.Instence.User.TranslationDirection == TranslationDirection.FromFirst
                ? Lesson.SelectedWord.Language2
                : Lesson.SelectedWord.Language1;
            if (HintLetters <= translation.Length)
            {
                State.Translation = translation.Substring(0, HintLetters);
            }
            CursorOnEnd = false;
        }

        private void Pause()
        {
            using (LessonStateBase lState = StateFactory.GetState(Lesson, LessonStateEnum.Pause))
            {
                LessonStateEnum stateEnumBeforePause = State.StateEnum;
                IInteractionProvider provider = new SimpleInfoProvider
                {
                    ViewModel = new InfoDialogViewModel
                    {
                        ButtonLabel = "Wznów",
                        Message = "Przerwa",
                        CloseAction = () =>
                        {
                            State = StateFactory.GetState(Lesson, stateEnumBeforePause);
                            if (State != null)
                            {
                                State.RefreshView();
                            }
                        }
                    }
                };
                State = lState;
                State.RefreshView();
                provider.Interact();
            }
        }

        private void StartLesson()
        {
            try
            {
                Lesson.Timer.StartTimer();
                Lesson.NextWord();
                State = StateFactory.GetState(Lesson, LessonStateEnum.NextWord);
                State.RefreshView();
            }
            catch (Exception lException)
            {
                logger.Error("{0} - {1}", "TeachViewModel.StartLesson", lException.Message);
            }
        }

        private void Correct()
        {
            if (Lesson == null || Lesson.SelectedWord == null)
                return;
            LessonStateEnum lastState = State.StateEnum;
            using (LessonStateBase lPauseState = StateFactory.GetState(Lesson, LessonStateEnum.Pause))
            {
                State = lPauseState;
                State.RefreshView();
                IInteractionProvider provider = new CorrectWordProvider()
                {
                    Word = Lesson.SelectedWord,
                    OnRemove = () =>
                    {
                        if (Lesson.Counter > Lesson.BeginWordsList.Count)
                        {
                            Lesson.ResultList.First(x => x.Group.Id == Lesson.SelectedWord.Group.Id).Wrong--;
                        }
                        Lesson.BeginWordsList.Remove(Lesson.SelectedWord);
                        Lesson.NextWord();
                        CheckNextState();
                    },
                    OnCorrect = () =>
                    {
                        State = StateFactory.GetState(Lesson, lastState);
                        State.RefreshView();
                    },
                    OnClose = () =>
                     {
                         State = StateFactory.GetState(Lesson, lastState);
                         State.RefreshView();
                     }
                };
                provider.Interact();
            }
        }

        private void BackAction()
        {
            LessonStateEnum lLastState = State.StateEnum;
            using (LessonStateBase lessonState = StateFactory.GetState(Lesson, LessonStateEnum.Pause))
            {
                IInteractionProvider provider = new YesNoProvider()
                {
                    ViewModel = new YesNoDialogViewModel()
                    {
                        DialogTitle = "Uwaga",
                        Message = "Czy na pewno chcesz opuścić lekcje?",
                        PositiveLabel = "Tak",
                        NegativeLabel = "Nie",
                        YesAction = () =>
                        {
                            //if (Lesson != null)
                            //{
                            //    ISerializer<Lesson> serializer = new BinarySerializer<Lesson>
                            //    {
                            //        Settings = new BinarySerializerSettings
                            //        {
                            //            Path = "lesson",
                            //            RemoveAfterRead = true,
                            //        }
                            //    };
                            //    try
                            //    {
                            //        serializer.Write(Lesson);
                            //    }
                            //    catch (Exception e)
                            //    {
                            //        LoggerSingleton.LogError("Błąd serializowania lekcji - {0}", e.Message);
                            //    }
                            //    Lesson.FinishLesson();
                            //    Lesson.Timer.StopTimer();
                            //}
                            Switcher.Back(true);
                        },
                        NoAction = () =>
                        {
                            State = StateFactory.GetState(Lesson, lLastState);
                            if (State != null)
                            {
                                State.RefreshView();
                            }
                        }
                    },
                };
                State = lessonState;
                State.RefreshView();
                provider.Interact();
            }
        }

        private void OnEnterClick()
        {
            if (Lesson.IsChecked)
            {
                if (Lesson.IsCorrect)
                {
                    Known();
                }
                else
                {
                    Unknown();
                }
            }
            else
            {
                Check();
            }
        }

        private void Known()
        {
            Lesson.Known();
            CheckNextState();
        }



        private void Unknown()
        {
            Lesson.Unknown();
            CheckNextState();
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
    }
}