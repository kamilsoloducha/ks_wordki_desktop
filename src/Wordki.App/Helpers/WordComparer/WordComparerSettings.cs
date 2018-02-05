using System;
using System.Collections.Generic;

namespace Wordki.Helpers.WordComparer
{
    [Serializable]
    public class WordComparerSettings : IWordComparerSettings
    {
        public ICollection<INotCheck> NotCheckers { get; set; }
        public char WordSeparator { get; set; }

        public WordComparerSettings()
        {
            NotCheckers = new List<INotCheck>();
        }
    }
}
