using Repository.Models.Enums;
using System;
using Wordki.Database;

namespace Wordki.Commands
{
    public class TranslationDirectionChangeAction
    {

        private Action action;
        public Action Action { get { return action; } }

        public TranslationDirectionChangeAction()
        {
            action = Execute;
        }

        private async void Execute()
        {
            IUserManager userManager = UserManagerSingleton.Instence;
            switch (userManager.User.TranslationDirection)
            {
                case TranslationDirection.FromFirst:
                    userManager.User.TranslationDirection = TranslationDirection.FromSecond;
                    break;
                case TranslationDirection.FromSecond:
                    userManager.User.TranslationDirection = TranslationDirection.FromFirst;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            await userManager.UpdateAsync();
        }
    }
}
