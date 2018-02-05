using WordkiModel;
using System.Collections.Generic;
using Wordki.Helpers.WordComparer;

namespace Wordki.Helpers.WordFinder
{
    public class WordFinder : IWordFinder
    {

        private List<long> returnedWordIds;
        private IEnumerable<IWord> Words { get; set; }
        private IWordComparer WordComparer { get; set; }

        public WordFinder(IEnumerable<IWord> words, IWordComparer wordComparer)
        {
            returnedWordIds = new List<long>();
            Words = words;
            WordComparer = wordComparer;
        }

        public IEnumerable<IWord> FindWords()
        {
            foreach (IWord word1 in Words)
            {
                foreach (IWord word2 in Words)
                {
                    if (!WordComparer.IsEqual(word1, word2))
                    {
                        continue;
                    }
                    if (returnedWordIds.Contains(word2.Id))
                    {
                        continue;
                    }
                    yield return word1;
                    yield return word2;
                }
            }
        }
    }
}
