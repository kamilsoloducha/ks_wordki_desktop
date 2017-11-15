using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.LessonScheduler
{
    public static class IntervalTimeCalculator
    {

        /// <summary>
        /// Return TimeSpan between dateTime parameter and now
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>TimeSpan between dates. If datetime parameter is happend before now, value will be negative</returns>
        public static TimeSpan GetIntervalBetweenNow(DateTime dateTime)
        {
            return dateTime.Subtract(DateTime.Now);
        }

        /// <summary>
        /// Return TimeSpan between dateTime parameter and today at 23:59:59
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>TimeSpan between dates. If datetime parameter is happend before today midnight, value will be negative</returns>
        public static TimeSpan GetIntervalBetweenTodayMidnight(DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));
        }

    }
}
