using System.Runtime.CompilerServices;

namespace WordkiRepository.StateManager {
  public interface IStateManager<T> {
    int NewState(int state, [CallerMemberName] string name = "");
    int GetState(int state, string name);
  }
}
