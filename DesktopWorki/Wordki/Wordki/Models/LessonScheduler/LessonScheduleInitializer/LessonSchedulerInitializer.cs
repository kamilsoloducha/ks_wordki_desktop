using WordkiModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.LessonScheduler
{
    public class LessonSchedulerInitializer2 : ILessonScheduleInitializer
    {
        private IEnumerable<int> _timeSeparator;
        public IEnumerable<int> TimeSeparator
        {
            get
            {
                return _timeSeparator;
            }
        }

        public TranslationDirection TranslationDirection { get; set; } 

        public LessonSchedulerInitializer2(IEnumerable<int> timeSeparator)
        {
            _timeSeparator = timeSeparator;
        }
    }
}
