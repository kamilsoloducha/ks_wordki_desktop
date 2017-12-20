using Wordki.Models.Lesson;

namespace Wordki.ViewModels.LessonStates
{
    public class PauseTypingState : LessonStateBase
    {

        public PauseTypingState(Lesson pLesson, LessonStateBase pLastState)
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
            StartStopButtonCommand = TeachViewModelBase.PauseCommand;
        }

        public override void RefreshView()
        {
            base.RefreshView();
            Lesson.Timer.Pause();
        }

        public override void Dispose()
        {
            Lesson.Timer.Resume();
        }

    }
}
