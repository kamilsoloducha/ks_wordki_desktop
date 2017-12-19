using System.Collections.Generic;
using WordkiModel;

namespace Wordki.InteractionProvider
{
    public interface ILanguageProvider : IInteractionProvider
    {

        LanguageType SelectedType { get; }
        IEnumerable<LanguageType> Items { get; set; }

    }
}
