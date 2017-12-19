using WordkiModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wordki.Helpers.WordComparer
{
    [Serializable]
    public class WordComparer : IWordComparer
    {
        public IWordComparerSettings Settings { get; set; }

        public WordComparer()
        {
        }

        public bool IsEqual(string word1, string word2)
        {
            IEnumerable<string> words1 = word1.Split(Settings.WordSeparator);
            IEnumerable<string> words2 = word2.Split(Settings.WordSeparator);
            if (CompareWordList(words1, words2))
            {
                return true;
            }

            foreach (INotCheck notCheck in Settings.NotCheckers)
            {
                words1 = words1.Select(x => notCheck.Convert(x));
                words2 = words2.Select(x => notCheck.Convert(x));
                if (CompareWordList(words1, words2))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEqual(IWord word1, IWord word2)
        {
            int isSame = 0;
            if (word1.Id == word2.Id)
                return false;
            if (isSame == 0 && IsEqual(word1.Language1, word2.Language1))
                isSame++;
            if (isSame == 0 && IsEqual(word1.Language2, word2.Language2))
                isSame++;
            return isSame == 0;
        }

        private bool CompareWordList(IEnumerable<string> list1, IEnumerable<string> list2)
        {
            foreach (string w1 in list1)
            {
                foreach (string w2 in list2)
                {
                    if (w1.Equals(w2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
