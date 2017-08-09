using System;
using System.Windows.Input;

namespace Wordki.Helpers {
  public class BuilderCommand : System.Windows.Input.ICommand {
    private Action<object> execute;

    private Predicate<object> canExecute;

    private event EventHandler CanExecuteChangedInternal;
    private string Login;

    public BuilderCommand(Action<object> execute)
      : this(execute, DefaultCanExecute) {
    }

    public BuilderCommand(Action<object> execute, Predicate<object> canExecute) {
      if (execute == null) {
        throw new ArgumentNullException("execute");
      }

      if (canExecute == null) {
        throw new ArgumentNullException("canExecute");
      }

      this.execute = execute;
      this.canExecute = canExecute;
    }

    public void SetExecute(Action<object> pExecute) {
      execute = pExecute;
    }

    public BuilderCommand(string Login) {
      // TODO: Complete member initialization
      this.Login = Login;
    }

    public event EventHandler CanExecuteChanged {
      add {
        CommandManager.RequerySuggested += value;
        CanExecuteChangedInternal += value;
      }

      remove {
        CommandManager.RequerySuggested -= value;
        CanExecuteChangedInternal -= value;
      }
    }

    public bool CanExecute(object parameter) {
      return canExecute != null && canExecute(parameter);
    }

    public void Execute(object parameter) {
      execute(parameter);
    }

    public void OnCanExecuteChanged() {
      EventHandler handler = CanExecuteChangedInternal;
      if (handler != null) {
        //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
        handler.Invoke(this, EventArgs.Empty);
      }
    }

    public void Destroy() {
      canExecute = _ => false;
      execute = _ => { return; };
    }

    private static bool DefaultCanExecute(object parameter) {
      return true;
    }
  }
}
