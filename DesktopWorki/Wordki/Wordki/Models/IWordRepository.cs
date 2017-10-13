using Repository.Models;
using System.Collections.Generic;

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

    }
}
