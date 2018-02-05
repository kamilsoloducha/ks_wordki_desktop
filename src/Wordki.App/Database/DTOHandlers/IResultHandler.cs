using System.Collections.Generic;
using Oazachaosu.Core.Common;

namespace Wordki.Database
{
    public interface IResultHandler
    {
        void Handle(IEnumerable<ResultDTO> resultsDto);
    }
}