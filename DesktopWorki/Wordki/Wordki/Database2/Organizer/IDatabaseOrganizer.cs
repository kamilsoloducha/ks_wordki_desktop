using System.Collections.Generic;

namespace Wordki.Database2
{
    public interface IDatabaseOrganizer
    {

        IEnumerable<string> GetDatabases();

    }
}
