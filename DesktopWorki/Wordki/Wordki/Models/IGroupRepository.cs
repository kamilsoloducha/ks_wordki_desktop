using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models
{
    public interface IGroupRepository
    {

        IEnumerable<Group> GetGroups();

        Group Get(long id);

        void Save(Group group);

        void Update(Group update);

        void Delete(Group group);

        long RowCount();

    }
}
