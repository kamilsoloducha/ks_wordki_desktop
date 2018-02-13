using System;
using Util.Collections;
using Wordki.Database;
using WordkiModel;

namespace Wordki.Commands
{
    public class SelectNextGroupAction
    {
        private readonly IDatabase database;
        private readonly IGroupSelectable groupSelectable;

        private Action action;
        public Action Action { get { return action; } }

        public SelectNextGroupAction(IGroupSelectable groupSelectable, IDatabase database)
        {
            action = Execute;
            this.database = database;
            this.groupSelectable = groupSelectable;
        }

        private void Execute()
        {
            IGroup nextGroup = database.Groups.Next(groupSelectable.SelectedGroup);
            if (nextGroup == null)
            {
                return;
            }
            groupSelectable.SelectedGroup = nextGroup;
        }
    }
}
