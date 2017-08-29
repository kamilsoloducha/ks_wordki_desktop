using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.Command
{
    public class SimpleCommandAsync : ICommand
    {
        private Func<Task<bool>> Action { get; set; }

        public SimpleCommandAsync(Func<Task<bool>> pAction)
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
