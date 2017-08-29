using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.Command
{
    public class SimpleCommand : ICommand
    {
        private Func<bool> Action { get; set; }

        public SimpleCommand(Func<bool> pAction)
        {
            Action = pAction;
        }

        public bool Execute()
        {
            try
            {
                Action.Invoke();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
