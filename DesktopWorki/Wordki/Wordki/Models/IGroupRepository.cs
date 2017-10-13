using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models
{
    public interface IGroupRepository
    {

        IEnumerable<IGroup> GetGroups();

        IGroup Get(long id);

        void Save(IGroup group);

        void Save(IEnumerable<IGroup> groups);

        void Update(IGroup group);

        void Update(IEnumerable<IGroup> groups);

        void Delete(IGroup group);

        long RowCount();

    }
}
