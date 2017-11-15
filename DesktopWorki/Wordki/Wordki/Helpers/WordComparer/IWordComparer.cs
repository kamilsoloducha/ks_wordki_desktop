using Wordki.Models;

namespace Wordki.Helpers.WordComparer
{
    public interface IWordComparer
    {
        IWordComparerSettings Settings { get; set; }

        bool IsEqual(Word word1, Word word2);

        bool IsEqual(string word1, string word2);
    }
}
