using System.Collections.Generic;

namespace Wordki.Helpers.WordComparer
{
    public interface IWordComparerSettings
    {

        ICollection<INotCheck> NotCheckers { get; set; }

        char WordSeparator { get; set; }

    }
}
