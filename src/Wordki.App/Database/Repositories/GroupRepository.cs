using NHibernate;
using System.Linq;
using System.Collections.Generic;
using WordkiModel;
using System.Threading.Tasks;
using Wordki.Database.Repositories;
using NLog;

namespace Wordki.Database
{
    public class GroupRepository : IGroupRepository
    {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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

        public async Task DeleteAsync(IGroup group)
        {
            if (!CheckObject(group))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                await session.DeleteAsync(group);
                transaction.Commit();
            }
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

        public async Task<IGroup> GetAsync(long id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IGroup group = await session.GetAsync<IGroup>(id);
                group.Words.ToArray();
                group.Results.ToArray();
                return group;
            }
        }

        public IEnumerable<IGroup> GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IEnumerable<IGroup> groups = session.Query<IGroup>().ToArray();
                foreach (IGroup group in groups)
                {
                    group.Words.ToArray();
                    group.Results.ToArray();
                }
                return groups;
            }
        }

        public Task<IEnumerable<IGroup>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public long RowCount()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<IGroup>().RowCountInt64();
            }
        }

        public async Task<long> RowCountAsync()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return await session.QueryOver<IGroup>().RowCountInt64Async();
            }
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
                session.SaveOrUpdate(group);
                transaction.Commit();
            }
        }

        public void Save(IEnumerable<IGroup> groups)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (IGroup group in groups)
                {
                    session.SaveOrUpdate(group);
                }
                transaction.Commit();
            }
        }

        public async Task SaveAsync(IGroup group)
        {
            if (!CheckObject(group))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                await session.SaveOrUpdateAsync(group);
                transaction.Commit();
            }
        }

        public async Task SaveAsync(IEnumerable<IGroup> groups)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (IGroup group in groups)
                {
                    await session.SaveOrUpdateAsync(group);
                }
                transaction.Commit();
            }
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
                session.Update("Group", group, group.Id);
                transaction.Commit();
            }
        }

        public void Update(IEnumerable<IGroup> groups)
        {
            if (groups == null || groups.Count() == 0)
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (IGroup group in groups)
                {
                    session.Update(group);
                }
                transaction.Commit();
            }
        }

        public async Task UpdateAsync(IGroup group)
        {
            if (!CheckObject(group))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                await session.UpdateAsync("Group", group, group.Id);
                transaction.Commit();
            }
        }

        public async Task UpdateAsync(IEnumerable<IGroup> groups)
        {
            if (groups == null || groups.Count() == 0)
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (IGroup group in groups)
                {
                    await session.UpdateAsync(group);
                }
                transaction.Commit();
            }
        }

        public bool CheckObject(IGroup group)
        {
            return group != null;
        }
    }
}
