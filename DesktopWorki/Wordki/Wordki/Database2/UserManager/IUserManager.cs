using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Database2
{
    public interface IUserManager
    {

        IUser User { get; }

        void Set(IUser user);
        void UnSet();
        void Update();

    }
}
