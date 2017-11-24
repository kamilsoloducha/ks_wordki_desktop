using System;
using System.Linq;
using Wordki.Helpers;
using Wordki.Models.Lesson;
using Repository.Models;
using Wordki.Database;
using Wordki.Views.Dialogs.Progress;
using Util.Threads;

namespace Wordki.ViewModels.LessonStates
{
    public class AfterEndTypingState : LessonStateBase
    {

        public AfterEndTypingState(Lesson pLesson, LessonStateBase pLastState)
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
            SimpleWork work = new SimpleWork();
            work.WorkFunc += SaveDatabase;
            BackgroundQueueWithProgressDialog worker = new BackgroundQueueWithProgressDialog();
            ProgressDialog dialog = new ProgressDialog();
            dialog.ViewModel = new Dialogs.Progress.ProgressDialogViewModel()
            {
                DialogTitle = "Zapisuje wyniki",
                CanCanceled = false,
            };
            worker.Dialog = dialog;
            worker.AddWork(work);
            worker.Execute();
            TeachViewModelBase._switcher.Back(true);
        }

        private WorkResult SaveDatabase()
        {
            IDatabase database = DatabaseSingleton.Instance;
            Lesson.FinishLesson();
            Lesson.Timer.StopTimer();
            DateTime now = DateTime.Now;
            foreach (IWord word in Lesson.BeginWordsList)
            {
                IWord wordFromGroup = word.Group.Words.FirstOrDefault(x => x.Id == word.Id);
                if (wordFromGroup != null)
                {
                    wordFromGroup.Drawer = word.Drawer;
                    wordFromGroup.RepeatingNumber++;
                    wordFromGroup.LastRepeating = now;
                }
                database.UpdateWord(word);
            }
            foreach (IResult result in Lesson.ResultList)
            {
                result.Group.Results.Add(result);
                database.AddResult(result);
            }
            return new WorkResult()
            {
                Success = true,
            };
        }

        protected override void RefreshFocused() { }
        protected override void RefreshDrawerLight()
        {
            SelectedDrawer = -1;
        }
        protected override void RefreshStartStopButton()
        {
            StartStopButtonContent = "Stop";
            StartStopButtonCommand = TeachViewModelBase.PauseCommand;
        }
    }

}