using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models
{
    public class ResultCalculator : IResultCalculator
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

    }
}
