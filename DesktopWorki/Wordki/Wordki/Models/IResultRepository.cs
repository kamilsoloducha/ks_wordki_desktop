using Repository.Models;
using System.Collections.Generic;

namespace Wordki.Models
{
    public interface IResultRepository
    {

        IEnumerable<IResult> GetWords();

        IResult Get(long id);

        void Save(IResult result);

        void Update(IResult result);

        void Delete(IResult result);

        long RowCount();

    }
}
