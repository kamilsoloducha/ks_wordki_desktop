using WordkiModel;
using System.Collections.Generic;
using Wordki.Helpers.WordComparer;

namespace Wordki.Helpers.WordFinder
{
    public class WordFinder : IWordFinder
    {

        private IEnumerable<IWord> Words { get; set; }
        private IWordComparer WordComparer { get; set; }

        public WordFinder(IEnumerable<IWord> words, IWordComparer wordComparer)
        {
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
                    yield return word1;
                    yield return word2;
                }
            }
        }
    }
}
