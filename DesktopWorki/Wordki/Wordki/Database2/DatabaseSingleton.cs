using Wordki.Models;

namespace Wordki.Database2
{
    public class DatabaseSingleton
    {

        private static IDatabase _instance;
        private static object obj = new object();

        public static IDatabase GetDatabase()
        {
            lock (obj)
            {
                if (_instance == null)
                {
                    _instance = new NHibernateDatabase();
                }
            }
            return _instance;
        }

    }
}
