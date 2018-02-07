using NLog;
using System;
using System.Linq;
using Util.Collections;
using Wordki.Database;
using WordkiModel;

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
                new RemoveGroupAction(groupSelectable, wordSelectable, database).Action();
                return;
            }
            if (wordSelectable.SelectedWord == null)
            {
                return;
            }
            IWord newWord = groupSelectable.SelectedGroup.Words.Next(wordSelectable.SelectedWord);
            if (newWord == null)
            {
                newWord = groupSelectable.SelectedGroup.Words.Previous(wordSelectable.SelectedWord);
            }
            wordSelectable.SelectedWord = newWord;
            await database.DeleteWordAsync(wordSelectable.SelectedWord);
        }

    }
}
