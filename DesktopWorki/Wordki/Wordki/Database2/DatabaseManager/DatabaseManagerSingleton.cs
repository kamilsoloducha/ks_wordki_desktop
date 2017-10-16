namespace Wordki.Database2
{
    public class DatabaseOrganizerSingleton
    {

        private static IDatabaseOrganizer _instance;
        private static object _lock = new object();

        public static IDatabaseOrganizer Get()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new DatabaseOrganizer(NHibernateHelper.DirectoryPath);
                }
            }
            return _instance;
        }


    }
}
