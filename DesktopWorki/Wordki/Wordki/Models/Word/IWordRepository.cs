using Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wordki.Models
{
    public interface IWordRepository
    {

        IEnumerable<IWord> GetWords();

        IWord Get(long id);

        void Save(IWord word);

        void Update(IWord word);

        void Delete(IWord word);

        long RowCount();

        Task<IEnumerable<IWord>> GetWordsAsync();

        Task<IWord> GetAsync(long id);

        Task SaveAsync(IWord word);

        Task UpdateAsync(IWord word);

        Task DeleteAsync(IWord word);

        Task<long> RowCountAsync();

    }
}
