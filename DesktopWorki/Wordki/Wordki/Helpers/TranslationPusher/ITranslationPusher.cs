using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.TranslationPusher
{
    public interface ITranslationPusher
    {

        void SetTranslation(IWord word);

    }
}
