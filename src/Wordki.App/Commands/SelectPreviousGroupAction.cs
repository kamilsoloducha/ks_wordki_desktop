using System;
using Util.Collections;
using Wordki.Database;
using WordkiModel;

namespace Wordki.Commands
{
    public class SelectPreviousGroupAction
    {

        private readonly IDatabase database;
        private readonly IGroupSelectable groupSelectable;

        private Action action;
        public Action Action { get { return action; } }

        public SelectPreviousGroupAction(IGroupSelectable groupSelectable, IDatabase database)
        {
            action = Execute;
            this.database = database;
            this.groupSelectable = groupSelectable;
        }

        private void Execute()
        {
            IGroup previousGroup = database.Groups.Previous(groupSelectable.SelectedGroup);
            if (previousGroup == null)
            {
                return;
            }
            groupSelectable.SelectedGroup = previousGroup;
        }
    }
}
