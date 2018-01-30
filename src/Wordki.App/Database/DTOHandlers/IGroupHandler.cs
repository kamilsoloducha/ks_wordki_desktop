using System.Collections.Generic;
using Oazachaosu.Core.Common;

namespace Wordki.Database
{
    public interface IGroupHandler
    {
        void Handle(IEnumerable<GroupDTO> groupsDto);
    }
}