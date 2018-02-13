﻿namespace Wordki.Commands
{
    public static class ActionsSingleton<T> where T : class, new()
    {
        private static T instance;
        private static object lockObj = new object();
        public static T Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
                return instance;
            }
        }
    }
}
