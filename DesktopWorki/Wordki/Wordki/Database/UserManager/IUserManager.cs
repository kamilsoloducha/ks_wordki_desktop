﻿using WordkiModel;
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
