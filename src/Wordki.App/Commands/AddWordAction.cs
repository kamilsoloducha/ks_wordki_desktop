using System;
using Wordki.Database;
using Wordki.Models;
using WordkiModel;
using WordkiModel.Extensions;

namespace Wordki.Commands
{
    public class AddWordAction
    {

        private readonly IDatabase database;
        private readonly IGroupSelectable groupSelectable;
        private readonly IWordSelectable wordSelectable;

        private Action action;
        public Action Action { get { return action; } }

        public AddWordAction(IGroupSelectable groupSelectable, IWordSelectable wordSelectable, IDatabase database)
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
                new AddGroupAction(groupSelectable, database).Action();
            }
            IWord word = new Word();
            groupSelectable.SelectedGroup.AddWord(word);
            await database.AddWordAsync(word);
            wordSelectable.SelectedWord = word;
        }

    }
}
