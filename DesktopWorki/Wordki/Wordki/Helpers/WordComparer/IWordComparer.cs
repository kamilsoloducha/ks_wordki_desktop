using WordkiModel;

namespace Wordki.Helpers.WordComparer
{
    public interface IWordComparer
    {
        IWordComparerSettings Settings { get; set; }

        bool IsEqual(IWord word1, IWord word2);

        bool IsEqual(string word1, string word2);
    }
}
