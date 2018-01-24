using Wordki.Models.Lesson;

namespace Wordki.ViewModels.LessonStates
{
    public class PauseFiszkiState : LessonStateBase
    {

        public PauseFiszkiState(Lesson pLesson, LessonStateBase pLastState)
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

        public new void Dispose()
        {
            Lesson.Timer.Resume();
        }

    }
}
