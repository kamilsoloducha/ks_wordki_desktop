using WordkiModel;
using System.Collections.Generic;
using Oazachaosu.Core.Common;

namespace Wordki.InteractionProvider
{
    public interface ITranslationProvider : IInteractionProvider
    {

        IWord Word { get; set; }
        TranslationDirection TranslationDirection { get; set; }
        IEnumerable<string> Items { get; set; }
    }
}
