using NLog;
using System;
using System.Linq;
using Util.Collections;
using Wordki.Database;
using WordkiModel;

namespace Wordki.Commands
{
    public class RemoveGroupAction
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDatabase database;
        private readonly IGroupSelectable groupSelectable;
        private readonly IWordSelectable wordSelectable;
        private Action action;
        public Action Action { get { return action; } }

        public RemoveGroupAction(IGroupSelectable groupSelectable, IWordSelectable wordSelectable, IDatabase database)
        {
            action = Execute;
            this.database = database;
            this.groupSelectable = groupSelectable;
            this.wordSelectable = wordSelectable;
        }

        private async void Execute()
        {
            if (groupSelectable.SelectedGroup == null)
            {
                logger.Info($"SelectedGroup is null. It is imposible to remove the group");
                return;
            }
            IGroup groupToRemove = groupSelectable.SelectedGroup;
            int groupIndex = database.Groups.IndexOf(groupSelectable.SelectedGroup);
            IGroup newSelected = database.Groups.Next(groupToRemove);
            if (newSelected == null)
            {
                newSelected = database.Groups.Previous(groupToRemove);
            }
            groupSelectable.SelectedGroup = newSelected;
            wordSelectable.SelectedWord = groupSelectable.SelectedGroup == null ? null : groupSelectable.SelectedGroup.Words.LastOrDefault();
            await database.DeleteGroupAsync(groupToRemove);
        }

    }
}
