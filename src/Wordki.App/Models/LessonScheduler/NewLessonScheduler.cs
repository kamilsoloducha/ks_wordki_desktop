using WordkiModel;
using System.Linq;
using System;

namespace Wordki.Models.LessonScheduler
{
    public class NewLessonScheduler : ILessonScheduler
    {
        public ILessonScheduleInitializer Initializer { get; set; }

        public int GetColor(IGroup group)
        {
            return 0;
        }

        public int GetTimeToLearn(IGroup group)
        {
            if(group == null)
            {
                throw new ArgumentNullException("Parameter group equals null");
            }
            if (group.Results.Count == 0)
            {
                return 0;
            }
            int specificDaysToLearn = 0;
            if (Initializer.TimeSeparator.Count() >= group.Results.Count)
            {
                specificDaysToLearn = Initializer.TimeSeparator.ElementAt(group.Results.Count - 1);
            }
            else
            {
                specificDaysToLearn = Initializer.TimeSeparator.Last();
            }
            return IntervalTimeCalculator.GetIntervalBetweenTodayMidnight(group.Results.Last().DateTime.AddDays(specificDaysToLearn)).Days;
        }
    }
}
