using NLog;
using System;
using Wordki.Database;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Commands
{
    public class AddGroupAction
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDatabase database;
        private readonly IGroupSelectable groupSelectable;

        private Action action;
        public Action Action { get { return action; } }

        public AddGroupAction(IGroupSelectable groupSelectable, IDatabase database)
        {
            action = Execute;
            this.database = database;
            this.groupSelectable = groupSelectable;
        }

        private async void Execute()
        {
            IGroup group = new Group();
            if (groupSelectable != null && groupSelectable.SelectedGroup != null)
            {
                group.Language1 = groupSelectable.SelectedGroup.Language1;
                group.Language2 = groupSelectable.SelectedGroup.Language2;
            }
            await database.AddGroupAsync(group);
        }
    }
}
