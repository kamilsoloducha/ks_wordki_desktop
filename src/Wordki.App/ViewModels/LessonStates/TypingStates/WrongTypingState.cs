using Oazachaosu.Core.Common;
using System.Windows.Media;

using Wordki.Models.Lesson;

namespace Wordki.ViewModels.LessonStates
{
    public class WrongTypingState : LessonStateBase
    {

        public WrongTypingState(Lesson pLesson, LessonStateBase pLastState)
          : base(pLesson, pLastState)
        {
            StateEnum = LessonStateEnum.Wrong;
        }

        protected override void RefreshGroupName()
        {
            GroupName = Lesson.SelectedWord.Group.Name;
        }

        protected override void RefreshTranslation()
        {
            switch (_translationDirection)
            {
                case TranslationDirection.FromFirst:
                    Translation = $"{Translation}\n{Lesson.SelectedWord.Language2}";
                    break;
                case TranslationDirection.FromSecond:
                    Translation = $"{Translation}\n{Lesson.SelectedWord.Language1}";
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
            StartStopButtonCommand = TeachViewModelBase.PauseCommand;
        }
    }
}