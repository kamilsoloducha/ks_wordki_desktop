using Repository.Models;
using System;

namespace Wordki.Commands
{
    public class CheckUncheckAction
    {
        private Action<IWord> action;
        public Action<IWord> Action { get { return action; } }

        public CheckUncheckAction()
        {
            action = Execute;
        }

        private async void Execute(IWord word)
        {
            if (word == null)
            {
                return;
            }
            word.Checked = !word.Checked;
            await Database.DatabaseSingleton.Instance.UpdateWordAsync(word);
        }
    }
}
