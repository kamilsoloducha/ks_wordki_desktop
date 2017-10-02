using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Helpers.WordComparer;
using Wordki.Models;

namespace Wordki.Helpers.WordFinder
{
    public class WordFinder : IWordFinder
    {

        private List<long> returnedWordIds;
        private IEnumerable<Word> Words { get; set; }
        private IWordComparer WordComparer { get; set; }

        public WordFinder(IEnumerable<Word> words, IWordComparer wordComparer)
        {
            returnedWordIds = new List<long>();
            Words = words;
            WordComparer = wordComparer;
        }

        public IEnumerable<Word> FindWords()
        {
            foreach (Word word1 in Words)
            {
                foreach (Word word2 in Words)
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
