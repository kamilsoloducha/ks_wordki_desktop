using System.Reflection;

namespace Wordki.Helpers {
  public static class Util {

    public static string GetTimeFromSeconds(int pSeconds) {
      int lHours = pSeconds / (60 * 60);
      int lMinutes = (pSeconds - 60 * 60 * lHours) / 60;
      int lSeconds = (pSeconds - 60 * 60 * lHours - 60 * lMinutes);
      return string.Format("{0:00}:{1:00}:{2:00}", lHours, lMinutes, lSeconds);
    }

    public static string GetAproximatedTimeFromSeconds(int pSeconds) {
      int lHours = pSeconds / (60 * 60);
      if (lHours > 10) {
        return string.Format("{0}h", lHours);
      }
      int lMinutes = (pSeconds - 60 * 60 * lHours) / 60;
      if (lHours > 0) {
        return string.Format("{0}h {1}m", lHours, lMinutes);
      }
      int lSeconds = (pSeconds - 60 * 60 * lHours - 60 * lMinutes);
      return string.Format("{0}m {1}s", lMinutes, lSeconds);
    }

    public static string GetExeFilePath() {
      string lExePath = Assembly.GetExecutingAssembly().Location;
      return lExePath.Substring(0, lExePath.LastIndexOf("\\"));
    }
  }
}
