using Oazachaosu.Core.Common;
using System.Collections.Generic;

namespace Wordki.InteractionProvider
{
    public interface ILanguageProvider : IInteractionProvider
    {

        LanguageType SelectedType { get; }
        IEnumerable<LanguageType> Items { get; set; }

    }
}
