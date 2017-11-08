using Repository.Models;
using Repository.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.InteractionProvider
{
    public interface ITranslationProvider
    {

        IWord Word { get; set; }
        TranslationDirection TranslationDirection { get; set; }
        IEnumerable<string> Items { get; set; }

        void Interact();
    }
}
