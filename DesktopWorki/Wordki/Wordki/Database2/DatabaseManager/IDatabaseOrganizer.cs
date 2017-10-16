using Repository.Models;
using System.Collections.Generic;

namespace Wordki.Database2
{
    public interface IDatabaseOrganizer
    {
        bool CheckDatabase(IUser user);
        bool AddDatabase(IUser user);
        bool RemoveDatabase(IUser user);
        IEnumerable<string> GetDatabases();
    }
}
