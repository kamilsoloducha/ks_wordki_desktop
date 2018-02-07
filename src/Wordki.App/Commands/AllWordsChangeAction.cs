using System;
using Wordki.Database;

namespace Wordki.Commands
{
    public class AllWordsChangeAction
    {

        private readonly IUserManager userManager;

        private Action action;
        public Action Action { get { return action; } }

        public AllWordsChangeAction(IUserManager userManager)
        {
            action = Execute;
            this.userManager = userManager;
        }

        private async void Execute()
        {
            userManager.User.AllWords = !userManager.User.AllWords;
            await userManager.UpdateAsync();
        }
    }
}
