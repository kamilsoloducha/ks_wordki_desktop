using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models.Lesson.WordComparer;

namespace Wordki.Models.Lesson.WordComparer
{
    public class LetterCaseNotCheck : INotCheck
    {
        public StringBuilder Convert(StringBuilder builder)
        {
            for (int i = 0; i < builder.Length; i++)
            {
                builder[i] = char.ToLower(builder[i]);
            }
            return builder;
        }
    }
}
