using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.GroupCreator
{
    public class GroupCreatorSettings
    {

        public char ElementSeparator { get; set; }
        public char WordSeparator { get; set; }

        public void SetSeparators(string elementSeparator, string wordSeparator)
        {
            ElementSeparator = GetSeparator(elementSeparator);
            WordSeparator = GetSeparator(wordSeparator);
        }

        private char GetSeparator(string separator)
        {
            char result = ';';
            Char.TryParse(separator, out result);
            return result;
        }
    }
}
