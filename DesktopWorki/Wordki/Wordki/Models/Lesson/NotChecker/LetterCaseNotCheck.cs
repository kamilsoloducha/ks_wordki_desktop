using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models.Lesson.WordComparer;

namespace Wordki.Models.Lesson.WordComparer
{
    [Serializable]
    public class LetterCaseNotCheck : INotCheck
    {
        public string Convert(string text)
        {
            return text.ToLower();
        }
    }
}
