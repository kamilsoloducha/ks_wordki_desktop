using System;
using Wordki.ViewModels.LessonStates;

namespace Wordki.ViewModels
{
    public class TeachTypingViewModel : TeachViewModelBase
    {

        public override void InitViewModel()
        {
            base.InitViewModel();
            EnableTextBox = true;
            StateFactory = new TypingLessonStateFactory();
            State = StateFactory.GetState(Lesson, LessonStateEnum.BeforeStart);
            State.RefreshView();
        }

        public override void Loaded()
        {
            
        }

        public override void Unloaded()
        {
            
        }

        protected override void Check(object obj)
        {
            HintLetters = 0;
            Lesson.Check(State.Translation);
            string translation = State.Translation;
            if (Lesson.IsCorrect)
            {
                State = StateFactory.GetState(Lesson, LessonStateEnum.Correct);
                State.RefreshView();
            }
            else
            {
                State = StateFactory.GetState(Lesson, LessonStateEnum.Wrong);
                State.Translation = translation;
                State.RefreshView();
            }
        }

    }
}
