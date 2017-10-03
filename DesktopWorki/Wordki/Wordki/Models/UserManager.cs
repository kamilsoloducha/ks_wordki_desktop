namespace Wordki.Models
{
    public class UserManager
    {

        private static UserManager _instance;
        private static object _instanceLock = new object();
        public User User { get; set; }

        public static UserManager GetInstance()
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new Models.UserManager();
                }
                return _instance;
            }
        }

        public User FindLoginedUser()
        {
            if (Settings.GetSettings().LastUserId > 0)
            {
                return Database.GetDatabase().GetUser(Settings.GetSettings().LastUserId);
            }
            return null;
        }

        public User FindLoginedUser(string pUserName, string pPassword)
        {
            return Database.GetDatabase().GetUser(pUserName, pPassword);
        }

        public void LoginUser(User pUser)
        {
            Settings.GetSettings().LastUserId = pUser.UserId;
            Settings.GetSettings().SaveSettings();
        }

        public void LogoutUser(User pUser)
        {
            Settings.GetSettings().LastUserId = -1;
            Settings.GetSettings().SaveSettings();
        }

    }
}
