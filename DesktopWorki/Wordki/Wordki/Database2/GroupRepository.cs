using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Database2
{
    public class GroupRepository : IGroupRepository
    {
        public void Delete(Group group)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(group);
                transaction.Commit();
            }
        }

        public Group Get(long id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                Group result = session.Get<Group>(id);
                result.Words.ToList();
                result.Results.ToList();
                return result;
            }
        }

        public IEnumerable<Group> GetGroups()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Query<Group>().ToList();
            }
        }

        public long RowCount()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Group>().RowCountInt64();
            }
        }

        public void Save(Group group)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(group);
                transaction.Commit();
            }
        }

        public void Update(Group group)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(group);
                transaction.Commit();
            }
        }
    }
}
