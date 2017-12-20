using System.Windows.Media;
using Wordki.Models.Lesson;

namespace Wordki.ViewModels.LessonStates
{
    public class CorrectTypingState : LessonStateBase
    {

        public CorrectTypingState(Lesson pLesson, LessonStateBase pLastState)
          : base(pLesson, pLastState)
        {
            StateEnum = LessonStateEnum.Correct;
        }

        protected override void RefreshGroupName()
        {
            GroupName = Lesson.SelectedWord.Group.Name;
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
            StartStopButtonCommand = TeachViewModelBase.PauseCommand;
        }
    }


}