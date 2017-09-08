using System;
using System.Collections.Generic;
using System.Linq;
using Wordki.Models.LessonScheduler.LessonScheduleInitializer;

namespace Wordki.Models.LessonScheduler
{
    public class LessonScheduler : ILessonScheduler
    {

        private ILessonScheduleInitializer _initializer;

        public LessonScheduler(ILessonScheduleInitializer initializer)
        {
            _initializer = initializer;
        }

        public int GetTimeToLearn(ICollection<Result> results)
        {
            return results.Any() ? GetTime(results.Count(), (int)IntervalTimeCalculator.GetIntervalBetweenTodayMidnight(results.Last().DateTime).TotalDays) : 0;
        }

        public int GetColor(Result lResult)
        {
            int days = lResult == null ? int.MaxValue : -(int)IntervalTimeCalculator.GetIntervalBetweenTodayMidnight(lResult.DateTime).TotalDays;
            if (days < 0)
            {
                return 4;
            }
            for (int i = 0; i < _initializer.TimeSeparator.Length; i++)
            {
                if (days < _initializer.TimeSeparator[i])
                {
                    return i;
                }
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
