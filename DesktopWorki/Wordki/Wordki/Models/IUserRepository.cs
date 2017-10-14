using Repository.Models;
using System.Collections.Generic;

namespace Wordki.Models
{
    public interface IUserRepository
    {

        IUser Get(string name, string password);

        void Save(IUser user);

        void Update(IUser user);

        long RowCount();

    }
}
