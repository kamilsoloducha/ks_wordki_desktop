using System;
using System.Linq;

namespace Wordki.Commands
{
    public class SelectLastWordAction
    {
        private readonly IGroupSelectable groupSelectable;
        private readonly IWordSelectable wordSelectable;

        private Action action;
        public Action Action { get { return action; } }

        public SelectLastWordAction(IGroupSelectable groupSelectable, IWordSelectable wordSelectable)
        {
            action = Execute;
            this.groupSelectable = groupSelectable;
            this.wordSelectable = wordSelectable;
        }

        private void Execute()
        {
            if (groupSelectable.SelectedGroup == null)
            {
                wordSelectable.SelectedWord = null;
            }
            else
            {
                wordSelectable.SelectedWord = groupSelectable.SelectedGroup.Words.LastOrDefault();
            }
        }
    }
}
