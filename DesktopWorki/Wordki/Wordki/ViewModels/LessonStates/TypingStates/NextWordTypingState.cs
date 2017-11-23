using System.Windows;
using System.Windows.Media;
using Wordki.Models;
using Wordki.Models.Lesson;
using Wordki.ViewModels.LessonStates;

namespace Wordki.ViewModels.LessonStates
{
    public class NextWordTypingState : LessonStateBase
    {

        public NextWordTypingState(Lesson pLesson, LessonStateBase pLastState)
          : base(pLesson, pLastState)
        {
            StateEnum = LessonStateEnum.NextWord;
        }

        protected override void RefreshGroupName()
        {
            GroupName = Lesson.SelectedWord.Group.Name;
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
            StartStopButtonCommand = TeachViewModelBase.PauseCommand;
        }
    }
}