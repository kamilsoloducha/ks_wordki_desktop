using Repository.Models;
using System.Threading.Tasks;

namespace Wordki.Database.Repositories
{
    public interface IUserRepository
    {

        IUser Get(string name, string password);

        void Save(IUser user);

        void Update(IUser user);

        long RowCount();

        Task<IUser> GetAsync(string name, string password);

        Task SaveAsync(IUser user);

        Task UpdateAsync(IUser user);

        Task<long> RowCountAsync();

    }
}
