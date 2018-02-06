using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Database;

namespace Wordki.Commands
{
    public class RemoveWordAction
    {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDatabase database;
        private readonly IGroupSelectable groupSelectable;
        private readonly IWordSelectable wordSelectable;
        private Action action;
        public Action Action { get { return action; } }

        public RemoveWordAction(IGroupSelectable groupSelectable, IWordSelectable wordSelectable, IDatabase database)
        {
            action = Execute;
            this.database = database;
            this.groupSelectable = groupSelectable;
            this.wordSelectable = wordSelectable;
        }

        private async void Execute()
        {
            if (wordSelectable.SelectedWord == null && groupSelectable.SelectedGroup != null)
            {
                new RemoveGroupAction(groupSelectable, wordSelectable, database);
                return;
            }
            if (await database.DeleteWordAsync(wordSelectable.SelectedWord))
            {
                wordSelectable.SelectedWord = groupSelectable.SelectedGroup.Words.LastOrDefault();
            }
        }

    }
}
