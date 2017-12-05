using Repository.Models;
using System;
using System.Windows.Input;

namespace Wordki.Commands
{
    public class CheckUncheckCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            IWord word = parameter as IWord;
            if(word == null)
            {
                return;
            }
            word.Checked = !word.Checked;
            Database.DatabaseSingleton.Instance.UpdateWordAsync(word);
        }
    }
}
