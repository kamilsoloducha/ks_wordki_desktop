using WordkiModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wordki.Database
{
    public interface IDatabaseOrganizer
    {
        bool CheckDatabase(IUser user);
        bool AddDatabase(IUser user);
        bool RemoveDatabase(IUser user);
        IEnumerable<string> GetDatabases();

        Task<bool> CheckDatabaseAsync(IUser user);
        Task<bool> AddDatabaseAsync(IUser user);
        Task<bool> RemoveDatabaseAsync(IUser user);

    }
}
