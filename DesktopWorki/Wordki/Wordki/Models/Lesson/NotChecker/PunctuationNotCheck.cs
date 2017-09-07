using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.Lesson.WordComparer
{
    public class PunctuationNotCheck : INotCheck
    {
        public StringBuilder Convert(StringBuilder text)
        {

            for (int i = text.Length - 1; i >= 0; i--)
            {
                char c = text[i];
                if (c == ' ' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c >= 128)
                {
                    continue;
                }
                text.Remove(i, 1);
            }
            return text;
        }
    }
}
