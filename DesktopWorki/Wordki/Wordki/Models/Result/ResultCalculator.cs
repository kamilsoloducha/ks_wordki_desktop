using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;

namespace Wordki.Models
{
    public class ResultCalculator : IResultCalculator
    {
        public int GetAcceptedCount(IEnumerable<IResult> results)
        {
            return results.Sum(x => x.Accepted);
        }

        public int GetAnswareCount(IEnumerable<IResult> results)
        {
            return results.Sum(x => x.Accepted + x.Correct + x.Wrong);
        }

        public int GetCorrectCount(IEnumerable<IResult> results)
        {
            return results.Sum(x => x.Correct);
        }

        public int GetWrongCount(IEnumerable<IResult> results)
        {
            return results.Sum(x => x.Wrong);
        }
    }
}
