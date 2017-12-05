using Repository.Models.Language;
using System.Collections.Generic;

namespace Wordki.InteractionProvider
{
    public interface ILanguageProvider : IInteractionProvider
    {

        LanguageType SelectedType { get; }
        IEnumerable<LanguageType> Items { get; set; }

    }
}
