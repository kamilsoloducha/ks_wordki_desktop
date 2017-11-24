﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Database
{
    public class UserManagerSingleton
    {

        private static IUserManager _instance;
        private static object _lock = new object();

        private UserManagerSingleton()
        {

        }

        public static IUserManager Instence
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new UserManager();
                    }
                }
                return _instance;
            }
        }

    }
}
