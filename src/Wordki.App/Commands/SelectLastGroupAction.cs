using System;
using System.Linq;
using Wordki.Database;

namespace Wordki.Commands
{
    public class SelectLastGroupAction
    {
        private readonly IDatabase database;
        private readonly IGroupSelectable groupSelectable;
        private readonly IWordSelectable wordSelectable;

        private Action action;
        public Action Action { get { return action; } }

        public SelectLastGroupAction(IGroupSelectable groupSelectable, IWordSelectable wordSelectable, IDatabase database)
        {
            action = Execute;
            this.database = database;
            this.groupSelectable = groupSelectable;
            this.wordSelectable = wordSelectable;
        }

        private void Execute()
        {
            groupSelectable.SelectedGroup = database.Groups.Last();
            wordSelectable.SelectedWord = groupSelectable.SelectedGroup?.Words.LastOrDefault();
        }
    }
}
