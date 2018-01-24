using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Helpers.WordConnector
{
    public interface IWordConnector
    {

        void Connect(IEnumerable<Word> words);

    }
}
