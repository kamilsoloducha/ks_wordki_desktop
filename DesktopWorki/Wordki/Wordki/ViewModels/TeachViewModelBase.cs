using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Repository.Models.Enums;
using Wordki.Helpers;
using Wordki.Models;
using Wordki.Models.Lesson;
using Wordki.Views.Dialogs;
using Repository.Models;
using Wordki.Database;
using Wordki.InteractionProvider;
using Wordki.ViewModels.Dialogs;
using Wordki.ViewModels.LessonStates;
using Wordki.Commands;

namespace Wordki.ViewModels
{
    public abstract partial class TeachViewModelBase : ViewModelBase, Util.Timer.ITimerListener
    {

        #region Properies
        public static Switcher _switcher;
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

        public ICommand UnknownCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand KnownCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand CorrectCommand { get; set; }
        public ICommand OnEnterClickCommand { get; set; }
        public ICommand CheckUncheckCommand { get; set; }
        public ICommand HintCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public static ICommand StartLessonCommand { get; set; }
        public static ICommand PauseCommand { get; set; }
        protected LessonStateFactoryBase StateFactory { get; set; }
        #endregion

        public TeachViewModelBase()
        {
            ActivateCommands();
            Settings = Settings.GetSettings();
            HintLetters = 0;
        }

        public override void InitViewModel()
        {
            _switcher = Switcher;
            Lesson lLesson = (Lesson)Util.PackageStore.Get(0);
            if (lLesson == null)
                return;
            lLesson.Timer.TimerListeners.Add(this);
            Timer = "";
            Lesson = lLesson;

            OnTimerTick(0);
            StartLessonCommand = new Util.BuilderCommand(StartLesson);
            PauseCommand = new Util.BuilderCommand(Pause);
        }

        public override void Back()
        {
            Lesson.Timer.TimerListeners.Remove(this);
        }

        protected abstract void Check(object obj);

        #region Commands
        private void ActivateCommands()
        {
            SearchCommand = new Util.BuilderCommand(Search);
            UnknownCommand = new Util.BuilderCommand(Unknown);
            CheckCommand = new Util.BuilderCommand(Check);
            KnownCommand = new Util.BuilderCommand(Known);
            BackCommand = new Util.BuilderCommand(BackAction);
            CorrectCommand = new Util.BuilderCommand(Correct);
            OnEnterClickCommand = new Util.BuilderCommand(OnEnterClick);
            CheckUncheckCommand = new CheckUncheckCommand();
            HintCommand = new Util.BuilderCommand(Hint);
        }

        private void Search(object obj)
        {
            ISearchProvider provider = new SearchProvider();
            provider.Interact();
        }

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

        private async void CheckUncheck(object obj)
        {
            if (Lesson.SelectedWord == null)
            {
                return;
            }
            IWord word = Lesson.SelectedWord.Group.Words.FirstOrDefault(x => x.Id == Lesson.SelectedWord.Id);
            if (word == null)
            {
                return;
            }
            word.Checked = !word.Checked;
            await DatabaseSingleton.Instance.UpdateWordAsync(word);
        }

        private void Pause(object obj)
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

        private void StartLesson(object obj)
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
                LoggerSingleton.LogError("{0} - {1}", "TeachViewModel.StartLesson", lException.Message);
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
        }



        private void Unknown(object obj)
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