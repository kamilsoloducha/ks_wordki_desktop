using NHibernate;
using Repository.Models;
using Wordki.Models;

namespace Wordki.Database2
{
    public class UserRepository : IUserRepository
    {

        public IUser Get(string name, string password)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<IUser>().Where(x => x.Name == name && x.Password == password).SingleOrDefault();
            }
        }

        public long RowCount()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<IUser>().RowCountInt64();
            }
        }

        public void Save(IUser user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }
        }

        public void Update(IUser user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(user);
                transaction.Commit();
            }
        }
    }
}
