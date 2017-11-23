using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.ViewModels.LessonStates
{
    public enum LessonStateEnum
    {
        None,
        BeforeStart,
        AfterEnd,
        NextWord,
        Correct,
        Wrong,
        Pause
    }
}
