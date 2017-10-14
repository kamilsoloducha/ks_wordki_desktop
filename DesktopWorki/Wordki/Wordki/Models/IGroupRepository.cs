using Repository.Models;
using System;
using System.Collections.Generic;

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
