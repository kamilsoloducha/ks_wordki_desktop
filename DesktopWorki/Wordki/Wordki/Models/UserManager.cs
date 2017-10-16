using Repository.Models;

namespace Wordki.Models
{
    public class UserManager
    {
        public IUser User { get; set; }

        public void LoginUser(User pUser)
        {
            Settings.GetSettings().LastUserId = pUser.LocalId;
            Settings.GetSettings().SaveSettings();
        }

        public void LogoutUser(User pUser)
        {
            Settings.GetSettings().LastUserId = -1;
            Settings.GetSettings().SaveSettings();
        }

    }
}
