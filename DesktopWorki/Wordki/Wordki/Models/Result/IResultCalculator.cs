using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models
{
    public interface IResultCalculator
    {
        int GetCorrectCount(IEnumerable<IResult> results);

        int GetAcceptedCount(IEnumerable<IResult> results);

        int GetWrongCount(IEnumerable<IResult> results);

        int GetAnswareCount(IEnumerable<IResult> results);
    }
}
