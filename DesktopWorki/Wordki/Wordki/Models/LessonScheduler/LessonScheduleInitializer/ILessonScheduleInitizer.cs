using Repository.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.LessonScheduler
{
    public interface ILessonScheduleInitializer
    {
        IEnumerable<int> TimeSeparator { get; }

        TranslationDirection TranslationDirection { get; set; }

    }
}
