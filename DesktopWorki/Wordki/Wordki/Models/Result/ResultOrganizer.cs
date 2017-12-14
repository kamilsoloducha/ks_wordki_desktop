using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models
{
    public class ResultOrganizer : IResultOrganizer
    {

        public IEnumerable<IGroup> Groups { get; set; }

        public int GetLessonTime(DateTime start, DateTime end)
        {
            if (Groups == null)
            {
                return 0;
            }
            int counter = 0;
            foreach (IGroup group in Groups)
            {
                foreach (IResult result in group.Results.Where(x => x.DateTime > start && x.DateTime < end))
                {
                    counter += result.TimeCount;
                }
            }
            return counter;
        }

        public int GetLessonTimeToday()
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            DateTime end = start.AddDays(1);
            return GetLessonTime(start, end);
        }

    }
}
