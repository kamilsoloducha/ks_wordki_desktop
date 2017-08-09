namespace Wordki.Models {
  public class UserManager {

    public User User { get; set; }

    public static User FindLoginedUser() {
      if (Settings.GetSettings().LastUserId > 0) {
        return Database.GetDatabase().GetUser(Settings.GetSettings().LastUserId);
      }
      return null;
    }

    public static User FindLoginedUser(string pUserName, string pPassword) {
        return Database.GetDatabase().GetUser(pUserName, pPassword);
    }

    public static void LoginUser(User pUser) {
      Settings.GetSettings().LastUserId = pUser.UserId;
      Settings.GetSettings().SaveSettings();
    }

    public static void LogoutUser(User pUser) {
      Settings.GetSettings().LastUserId = -1;
      Settings.GetSettings().SaveSettings();
    }

  }
}
