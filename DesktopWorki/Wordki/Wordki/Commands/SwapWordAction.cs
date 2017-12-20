using WordkiModel;
using System;

namespace Wordki.Commands
{
    public class SwapWordAction
    {

        private Action<IWord> action;

        public Action<IWord> Action { get { return action; } }

        public SwapWordAction()
        {
            action = Execute;
        }

        private void Execute(IWord word)
        {
            if (word == null)
            {
                return;
            }
            word.SwapLanguage();
        }
    }
}
