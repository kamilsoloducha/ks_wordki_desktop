using Wordki.Helpers;
using Wordki.Helpers.Command;

namespace Wordki.Models.RemoteDatabase {
  abstract public class RemoteDatabaseAbs {

    public static RemoteDatabaseAbs GetRemoteDatabase(User user) {
      Logger.LogInfo("Wybór RemoteDatabase: {0}", user.IsRegister ? "Remote" : "Fake");
      if (user.IsRegister) {
        return new RemoteDatabase();
      }
      return new RemoteDatabaseFake();
    }

    public abstract CommandQueue<ICommand> GetDownloadQueue();
    public abstract CommandQueue<ICommand> GetUploadQueue();

  }
}
