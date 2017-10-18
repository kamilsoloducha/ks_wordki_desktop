using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.Lesson.WordComparer
{
    [Serializable]
    public class WordComparer : IWordComparer
    {

        public ICollection<INotCheck> NotCheckers { get; private set; }

        public WordComparer()
        {
            NotCheckers = new List<INotCheck>();
        }

        public bool Compare(string word1, string word2)
        {
            IEnumerable<string> words1 = word1.Split(',');
            IEnumerable<string> words2 = word2.Split(',');
            if(CompareWordList(words1, words2))
            {
                return true;
            }
            
            foreach(INotCheck notCheck in NotCheckers)
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
