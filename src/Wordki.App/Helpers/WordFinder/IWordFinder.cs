using WordkiModel;
using System.Collections.Generic;

namespace Wordki.Helpers.WordFinder
{
    public interface IWordFinder
    {

        IEnumerable<IWord> FindWords();

    }
}
