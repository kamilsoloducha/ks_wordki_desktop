using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.Lesson.WordComparer
{
    public class WordComparer2 : IWordComparer
    {

        public ICollection<INotCheck> NotChecker { get; set; }

        public bool Compare(string word1, string word2)
        {
            
            return true;
        }
    }
}
