using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wordki.Database.Repositories
{
    public interface IRepository<T>
    {

        IEnumerable<T> GetAll();

        T Get(long id);

        void Save(T item);

        void Save(IEnumerable<T> items);

        void Update(T item);

        void Update(IEnumerable<T> items);

        void Delete(T item);

        long RowCount();


        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetAsync(long id);
        Task SaveAsync(T item);
        Task SaveAsync(IEnumerable<T> items);
        Task UpdateAsync(T item);
        Task UpdateAsync(IEnumerable<T> items);
        Task DeleteAsync(T item);
        Task<long> RowCountAsync();

    }
}
