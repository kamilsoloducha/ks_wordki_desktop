using NLog;
using System;
using Util.Collections;
using WordkiModel;

namespace Wordki.Commands
{
    public class SelectNextWordAction
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IGroupSelectable groupSelectable;
        private readonly IWordSelectable wordSelectable;
        private Action action;
        public Action Action { get { return action; } }

        public SelectNextWordAction(IGroupSelectable groupSelectable, IWordSelectable wordSelectable)
        {
            this.groupSelectable = groupSelectable;
            this.wordSelectable = wordSelectable;
            action = Execute;
        }

        private void Execute()
        {
            if (groupSelectable.SelectedGroup == null)
            {
                return;
            }
            IWord previousWord = groupSelectable.SelectedGroup.Words.Next(wordSelectable.SelectedWord);
            if (previousWord != null)
            {
                wordSelectable.SelectedWord = previousWord;
            }
        }
    }
}
