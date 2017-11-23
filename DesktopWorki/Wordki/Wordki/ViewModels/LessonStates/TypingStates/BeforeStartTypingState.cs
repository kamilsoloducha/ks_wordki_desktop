using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models.Lesson;

namespace Wordki.ViewModels.LessonStates
{
    public class BeforeStartTypingState : LessonStateBase
    {

        public BeforeStartTypingState(Lesson pLesson, LessonStateBase pLastState)
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
            StartStopButtonCommand = TeachViewModelBase.StartLessonCommand;
        }

        protected override void RefreshResult()
        {
            base.RefreshResult();
            Counter[0] = 0;
        }

    }
}
