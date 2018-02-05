using System;
using System.Linq;
using Wordki.Database;

namespace Wordki.Commands
{
    public class SelectLastGroupAction<T> where T : IGroupSelectable, IWordSelectable
    {
        private readonly IDatabase database;
        private readonly T selectable;

        private Action action;
        public Action Action { get { return action; } }

        public SelectLastGroupAction(T selectable, IDatabase database)
        {
            action = Execute;
            this.database = database;
            this.selectable = selectable;
        }

        private void Execute()
        {
            selectable.SelectedGroup = database.Groups.Last();
            selectable.SelectedWord = selectable.SelectedGroup?.Words.LastOrDefault();
        }
    }
}
