using System;
using Wordki.Models.Lesson;

namespace Wordki.ViewModels.LessonStates
{
    public class TypingLessonStateFactory : LessonStateFactoryBase
    {
        protected override LessonStateBase CreateState(Lesson pLesson, LessonStateEnum pStateEnum)
        {
            LessonStateBase lastState;
            LessonStates.TryGetValue(_selectedState, out lastState);
            switch (pStateEnum)
            {
                case LessonStateEnum.BeforeStart:
                    return new BeforeStartTypingState(pLesson, lastState);
                case LessonStateEnum.AfterEnd:
                    return new AfterEndTypingState(pLesson, lastState);
                case LessonStateEnum.NextWord:
                    return new NextWordTypingState(pLesson, lastState);
                case LessonStateEnum.Correct:
                    return new CorrectTypingState(pLesson, lastState);
                case LessonStateEnum.Wrong:
                    return new WrongTypingState(pLesson, lastState);
                case LessonStateEnum.Pause:
                    return new PauseTypingState(pLesson, lastState);
                default:
                    throw new ArgumentOutOfRangeException("pStateEnum");
            }
        }
    }
}
