using System.Collections.Generic;
using Oazachaosu.Core.Common;

namespace Wordki.Database
{
    public interface IWordHandler
    {
        void Handle(IEnumerable<WordDTO> wordsDto);
    }
}