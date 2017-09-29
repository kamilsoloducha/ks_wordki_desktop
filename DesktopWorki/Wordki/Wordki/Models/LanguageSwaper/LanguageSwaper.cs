using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.LanguageSwaper
{
    public class LanguageSwaper : ILanguageSwaper
    {

        public void Swap(Group group)
        {
            if (group == null)
            {
                return;
            }
            group.SwapLanguage();
            foreach (var word in group.WordsList)
            {
                word.SwapLanguages();
            }
            foreach (var result in group.ResultsList)
            {
                result.SwapDirection();
            }
        }

    }
}
