using System.Windows;
using System.Windows.Media;
using Wordki.Models;
using Wordki.Models.Lesson;

namespace Wordki.ViewModels.LessonStates
{
    public class NextWordFiszkiState : LessonStateBase
    {

        public NextWordFiszkiState(Lesson pLesson, LessonStateBase pLastState)
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

        protected override void RefreshTranslationBoxEnabled() { }

        protected override void RefreshTranslationColor()
        {
            TranslationColor = Application.Current.Resources["UsedNormalFrontBrush"] as Brush;
        }

        protected override void RefreshFocused() { }

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