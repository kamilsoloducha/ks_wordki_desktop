using WordkiModel;
using System.Collections.Generic;
using System.Linq;

namespace Wordki.Models.LessonScheduler
{
    public class LessonScheduler : ILessonScheduler
    {

        private ILessonScheduleInitializer _initializer;

        public LessonScheduler(ILessonScheduleInitializer initializer)
        {
            _initializer = initializer;
        }

        public int GetTimeToLearn(IGroup group)
        {
            return group.Results.Any()
                ? GetTime(group.Results.Count(),
                (int)IntervalTimeCalculator.GetIntervalBetweenTodayMidnight(
                    group.Results.Last(x => x.TranslationDirection == _initializer.TranslationDirection).DateTime).TotalDays) 
                : 0;
        }

        public int GetColor(IGroup group)
        {
            if (group == null)
            {
                return 4;
            }
            int days = group.Results.Count == 0 ? int.MaxValue : -(int)IntervalTimeCalculator.GetIntervalBetweenTodayMidnight(group.Results.Last().DateTime).TotalDays;
            if (days < 0)
            {
                return 4;
            }
            int index = 0;
            foreach(int item in _initializer.TimeSeparator)
            {
                if (days < item)
                {
                    return index;
                }
                index++;
            }
            return 4;
        }

        /// <summary>
        /// Return how many days have to passed until lesson is ready to use
        /// </summary>
        /// <param name="resultCount">How many results, the lesson have</param>
        /// <param name="difference">Difference bettwen last lesson and now</param>
        /// <returns></returns>
        private int GetTime(int resultCount, int difference)
        {
            int lTime;
            if (resultCount > 0 && resultCount <= 2)
            {
                lTime = 1 - difference;
            }
            else if (resultCount > 2 && resultCount <= 4)
            {
                lTime = 2 - difference;
            }
            else if (resultCount > 4 && resultCount <= 6)
            {
                lTime = 3 - difference;
            }
            else if (resultCount > 4 && resultCount <= 6)
            {
                lTime = 4 - difference;
            }
            else
            {
                lTime = 5 - difference;
            }
            return lTime < 0 ? 0 : lTime;
        }
    }
}
