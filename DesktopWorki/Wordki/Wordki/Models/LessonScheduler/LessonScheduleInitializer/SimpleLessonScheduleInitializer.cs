using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.LessonScheduler.LessonScheduleInitializer
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
    }
}
