using Repository.Helper;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Commands
{
    public class SwapWordsInGroupAction
    {

        private Action<IGroup> action;

        public Action<IGroup> Action { get { return action; } }

        public SwapWordsInGroupAction()
        {
            action = Execute;
        }

        private void Execute(IGroup group)
        {
            if (group == null)
            {
                return;
            }
            ILanguageSwaper swaper = new LanguageSwaper();
            swaper.Swap(group);
        }
    }
}
