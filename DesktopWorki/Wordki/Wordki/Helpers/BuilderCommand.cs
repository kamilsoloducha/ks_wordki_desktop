using System;
namespace Wordki.Helpers
{
    public class BuilderCommand : System.Windows.Input.ICommand
    {
        private Action<object> _action { get; }
        private Predicate<object> _canExecute { get; }

        public BuilderCommand(Action<object> action, Predicate<object> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public BuilderCommand(Action<object> action) : this(action, DefaultCanExecute)
        {
        }

        public event EventHandler CanExecuteChanged
        {
            add { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute != null && _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (_action != null)
            {
                _action(parameter);
            }
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
