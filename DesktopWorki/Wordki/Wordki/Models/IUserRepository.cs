using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models
{
    public interface IUserRepository
    {

        IEnumerable<User> GetUsers();

        User Get(long id);

        void Save(User user);

        void Update(User user);

        void Delete(User user);

        long RowCount();

    }
}
