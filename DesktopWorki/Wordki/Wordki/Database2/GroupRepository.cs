using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System.Collections.Generic;
using Wordki.Models;
using Repository.Models;

namespace Wordki.Database2
{
    public class GroupRepository : IGroupRepository
    {
        public void Delete(IGroup group)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(group);
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

        public long RowCount()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<IGroup>().RowCountInt64();
            }
        }

        public void Save(IGroup group)
        {
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

        public void Update(IGroup group)
        {
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
    }
}
