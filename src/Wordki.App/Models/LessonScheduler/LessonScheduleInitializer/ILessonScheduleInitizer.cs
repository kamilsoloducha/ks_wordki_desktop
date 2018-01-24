using WordkiModel.Enums;
using System.Collections.Generic;

namespace Wordki.Models.LessonScheduler
{
    public interface ILessonScheduleInitializer
    {
        IEnumerable<int> TimeSeparator { get; }

        TranslationDirection TranslationDirection { get; set; }

    }
}
