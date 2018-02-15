using NLog;
using System;
using Util.Collections;
using WordkiModel;

namespace Wordki.Commands
{
    public class SelectPreviousWordAction
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IGroupSelectable groupSelectable;
        private readonly IWordSelectable wordSelectable;
        private Action action;
        public Action Action { get { return action; } }

        public SelectPreviousWordAction(IGroupSelectable groupSelectable, IWordSelectable wordSelectable)
        {
            this.groupSelectable = groupSelectable;
            this.wordSelectable = wordSelectable;
            action = Execute;
        }

        private void Execute()
        {
            if (groupSelectable.SelectedGroup == null)
            {
                logger.Debug($"Selected group is null");
                return;
            }
            IWord previousWord = groupSelectable.SelectedGroup.Words.Previous(wordSelectable.SelectedWord);
            logger.Trace($"Selected word: '{wordSelectable.SelectedWord}', previous word: {previousWord}");
            if (previousWord != null)
            {
                wordSelectable.SelectedWord = previousWord;
            }
        }
    }
}
