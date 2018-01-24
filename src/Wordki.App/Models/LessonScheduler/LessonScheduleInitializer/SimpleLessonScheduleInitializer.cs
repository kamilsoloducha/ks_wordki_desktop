using Oazachaosu.Core.Common;
using System.Collections.Generic;


namespace Wordki.Models.LessonScheduler
{
    public class SimpleLessonScheduleInitializer : ILessonScheduleInitializer
    {
        private static int[] _timeSeparator = { 2, 4, 6, 8 };

        public IEnumerable<int> TimeSeparator
        {
            get
            {
                return _timeSeparator;
            }
        }

        public TranslationDirection TranslationDirection { get; set; }
    }
}
