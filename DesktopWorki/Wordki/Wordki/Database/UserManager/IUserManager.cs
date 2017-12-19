using WordkiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Database
{
    public interface IUserManager
    {

        IUser User { get; }

        void Set(IUser user);
        void UnSet();
        void Update();
        Task UpdateAsync();

    }
}
