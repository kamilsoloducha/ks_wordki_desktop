using NLog;
using System;
using System.Linq;
using Wordki.Database;
using WordkiModel;

namespace Wordki.Commands
{
    public class RemoveGroupAction<T> where T : IGroupSelectable, IWordSelectable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDatabase database;
        private readonly T selectable;
        private Action action;
        public Action Action { get { return action; } }

        public RemoveGroupAction(T selectable, IDatabase database)
        {
            action = Execute;
            this.database = database;
            this.selectable = selectable;
        }

        private async void Execute()
        {
            if (selectable.SelectedGroup == null)
            {
                logger.Info($"SelectedGroup is null. It is imposible to remove the group");
                return;
            }
            IGroup groupToRemove = selectable.SelectedGroup;
            int groupIndex = database.Groups.IndexOf(selectable.SelectedGroup);
            selectable.SelectedGroup = database.Groups.Count > groupIndex ? database.Groups[groupIndex] : null;
            selectable.SelectedWord = selectable.SelectedGroup?.Words.LastOrDefault();
            await database.DeleteGroupAsync(groupToRemove);
        }

    }
}
