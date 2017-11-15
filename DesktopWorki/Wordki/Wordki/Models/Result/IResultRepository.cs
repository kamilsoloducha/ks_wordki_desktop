using Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wordki.Models
{
    public interface IResultRepository
    {

        IEnumerable<IResult> GetResults();

        IResult Get(long id);

        void Save(IResult result);

        void Update(IResult result);

        void Delete(IResult result);

        long RowCount();

        Task<IEnumerable<IResult>> GetResultsAsync();

        Task<IResult> GetAsync(long id);

        Task SaveAsync(IResult result);

        Task UpdateAsync(IResult result);

        Task DeleteAsync(IResult result);

        Task<long> RowCountAsync();


    }
}
