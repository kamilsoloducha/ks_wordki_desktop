using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System.Collections.Generic;
using Wordki.Models;
using Repository.Models;
using System.Threading.Tasks;

namespace Wordki.Database2
{
    public class GroupRepository : IGroupRepository
    {
        public void Delete(IGroup group)
        {
            if (!CheckObject(group))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(group);
                transaction.Commit();
            }
        }

        public Task DeleteAsync(IGroup group)
        {
            return Task.Run(() => Delete(group));
        }

        public IGroup Get(long id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IGroup group = session.Get<IGroup>(id);
                group.Words.ToArray();
                group.Results.ToArray();
                return group;
            }
        }

        public Task<IGroup> GetAsync(long id)
        {
            return Task.Run(() => Get(id));
        }

        public IEnumerable<IGroup> GetGroups()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IEnumerable<IGroup> groups = session.Query<IGroup>().ToArray();
                foreach(IGroup group in groups)
                {
                    group.Words.ToArray();
                    group.Results.ToArray();
                }
                return groups;
            }
        }

        public Task<IEnumerable<IGroup>> GetGroupsAsync()
        {
            return Task.Run(() => GetGroups());
        }

        public long RowCount()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<IGroup>().RowCountInt64();
            }
        }

        public Task<long> RowCountAsync()
        {
            return Task.Run(() => RowCount());
        }

        public void Save(IGroup group)
        {
            if (!CheckObject(group))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(group);
                transaction.Commit();
            }
        }

        public void Save(IEnumerable<IGroup> groups)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach(IGroup group in groups)
                {
                    session.Save(group);
                }
                transaction.Commit();
            }
        }

        public Task SaveAsync(IGroup group)
        {
            return Task.Run(() => Save(group));
        }

        public Task SaveAsync(IEnumerable<IGroup> groups)
        {
            return Task.Run(() => Save(groups));
        }

        public void Update(IGroup group)
        {
            if (!CheckObject(group))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(group);
                transaction.Commit();
            }
        }

        public void Update(IEnumerable<IGroup> groups)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach(IGroup group in groups)
                {
                    session.Update(group);
                }
                transaction.Commit();
            }
        }

        public Task UpdateAsync(IGroup group)
        {
            return Task.Run(() => Update(group));
        }

        public Task UpdateAsync(IEnumerable<IGroup> groups)
        {
            return Task.Run(() => Update(groups));
        }

        public bool CheckObject(IGroup group)
        {
            return group != null;
        }
    }
}
