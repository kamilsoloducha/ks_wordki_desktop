using System;
using Wordki.Database;

namespace Wordki.Commands
{
    public class AllWordsChangeAction
    {

        private Action action;
        public Action Action { get { return action; } }

        public AllWordsChangeAction()
        {
            action = Execute;
        }

        private async void Execute()
        {
            IUserManager userManager = UserManagerSingleton.Instence;
            userManager.User.AllWords = !userManager.User.AllWords;
            await userManager.UpdateAsync();
        }
    }
}
