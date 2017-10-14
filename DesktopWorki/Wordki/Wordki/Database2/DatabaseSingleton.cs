using Wordki.Models;

namespace Wordki.Database2
{
    public class DatabaseSingleton
    {

        private IDatabase _instance;
        private object obj = new object();

        public IDatabase GetDatabase()
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
