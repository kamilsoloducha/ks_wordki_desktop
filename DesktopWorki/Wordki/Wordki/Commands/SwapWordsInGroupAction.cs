using WordkiModel;
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
            group.SwapLanguage();
        }
    }
}
