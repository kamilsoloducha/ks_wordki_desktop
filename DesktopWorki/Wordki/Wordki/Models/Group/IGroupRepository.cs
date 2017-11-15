using Repository.Models;
using System;
using System.Collections.Generic;
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


        Task<IEnumerable<IGroup>> GetGroupsAsync();

        Task<IGroup> GetAsync(long id);
        Task SaveAsync(IGroup group);
        Task SaveAsync(IEnumerable<IGroup> groups);
        Task UpdateAsync(IGroup group);
        Task UpdateAsync(IEnumerable<IGroup> groups);
        Task DeleteAsync(IGroup group);
        Task<long> RowCountAsync();

    }
}
