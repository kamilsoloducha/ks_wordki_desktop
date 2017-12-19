using WordkiModel;
using System.Collections.Generic;

namespace Wordki.Helpers.GroupConnector
{
    public interface IGroupConnector
    {

        IGroup DestinationGroup { get; }

        bool Connect(IList<IGroup> groups);

    }
}
