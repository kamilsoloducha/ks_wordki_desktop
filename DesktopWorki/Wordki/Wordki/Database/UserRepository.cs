using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;

namespace Wordki.Database2
{
    public class UserRepository : IUserRepository
    {
        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public User Get(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsers()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Query<User>();
            }
        }

        public long RowCount()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<User>().RowCountInt64();
            }
        }

        public void Save(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
