using Repository.Helper;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ILanguageSwaper swaper = new LanguageSwaper();
            swaper.Swap(word);
        }
    }
}
