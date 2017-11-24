using Wordki.Models;

namespace Wordki.Database
{
    public class DatabaseSingleton
    {
        private static bool TEST = false;
        private static IDatabase _instance;
        private static object obj = new object();

        public static IDatabase Instance
        {
            get
            {
                lock (obj)
                {
                    if (_instance == null)
                    {
                        _instance = Create();
                    }
                }
                return _instance;
            }
        }

        private static IDatabase Create()
        {
            if (TEST)
            {
                return new MemoryDatabase();
            }
            else
            {
                return new NHibernateDatabase();
            }
        }

    }
}
