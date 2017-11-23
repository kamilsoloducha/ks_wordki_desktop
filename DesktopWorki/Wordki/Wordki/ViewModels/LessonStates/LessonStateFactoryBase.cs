using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models.Lesson;

namespace Wordki.ViewModels.LessonStates
{
    public abstract class LessonStateFactoryBase
    {
        protected readonly Dictionary<LessonStateEnum, LessonStateBase> LessonStates = new Dictionary<LessonStateEnum, LessonStateBase>();
        protected LessonStateEnum _selectedState = LessonStateEnum.None;

        public LessonStateBase GetState(Lesson pLesson, LessonStateEnum pStateEnum)
        {
            
            _selectedState = pStateEnum;

            if (!LessonStates.ContainsKey(pStateEnum))
            {
                LessonStateBase lState = CreateState(pLesson, pStateEnum);
                LessonStates.Add(pStateEnum, lState);
                return lState;
            }
            return LessonStates[pStateEnum];
        }

        protected abstract LessonStateBase CreateState(Lesson pLesson, LessonStateEnum pStateEnum);
    }
}
