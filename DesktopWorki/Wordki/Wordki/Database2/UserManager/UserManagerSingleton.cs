namespace Wordki.Database2
{
    public class UserOrganizerSingleton
    {

        private static IUserOrganizer _instance;
        private static object _lock = new object();

        public static IUserOrganizer Get()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new UserOrganizer();
                }
            }
            return _instance;
        }


    }
}
