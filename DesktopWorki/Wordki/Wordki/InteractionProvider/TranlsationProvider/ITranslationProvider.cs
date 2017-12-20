using WordkiModel;
using WordkiModel.Enums;
using System.Collections.Generic;

namespace Wordki.InteractionProvider
{
    public interface ITranslationProvider : IInteractionProvider
    {

        IWord Word { get; set; }
        TranslationDirection TranslationDirection { get; set; }
        IEnumerable<string> Items { get; set; }
    }
}
