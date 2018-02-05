using WordkiModel;
using System.Collections.Generic;

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
