using System;
using Wordki.Models.Lesson;

namespace Wordki.ViewModels.LessonStates
{
    public class FiszkiLessonStateFactory : LessonStateFactoryBase
    {
        protected override LessonStateBase CreateState(Lesson pLesson, LessonStateEnum pStateEnum)
        {
            LessonStateBase lastState;
            LessonStates.TryGetValue(_selectedState, out lastState);
            switch (pStateEnum)
            {
                case LessonStateEnum.BeforeStart:
                    return new BeforeStartFiszkiState(pLesson, lastState);
                case LessonStateEnum.AfterEnd:
                    return new AfterEndFiszkiState(pLesson, lastState);
                case LessonStateEnum.NextWord:
                    return new NextWordFiszkiState(pLesson, lastState);
                case LessonStateEnum.Correct:
                    return new CorrectFiszkiState(pLesson, lastState);
                case LessonStateEnum.Wrong:
                    return new WrongFiszkiState(pLesson, lastState);
                case LessonStateEnum.Pause:
                    return new PauseFiszkiState(pLesson, lastState);
                default:
                    throw new ArgumentOutOfRangeException("pStateEnum");
            }
        }
    }
}
