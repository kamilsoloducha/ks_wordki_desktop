using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models.Lesson.WordComparer
{
    public class PunctuationNotCheck : INotCheck
    {
        private static StringBuilder _builder = new StringBuilder();

        public string Convert(string text)
        {
            _builder.Clear();
            foreach (char c in text.ToCharArray())
            {
                if (c == ' ' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c >= 128)
                {
                    _builder.Append(c);
                }

            }
            return _builder.ToString();
        }
    }
}
