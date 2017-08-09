using System.Runtime.CompilerServices;

namespace Wordki.Models.StateManager {
  public interface IStateManager<T> {
    int NewState(int state, [CallerMemberName] string name = "");
    int GetState(int state, string name);
  }
}
