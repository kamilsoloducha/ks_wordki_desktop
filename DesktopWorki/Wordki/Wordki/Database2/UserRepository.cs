using System;
using System.Threading.Tasks;
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

        public Task<IUser> GetAsync(string name, string password)
        {
            return Task.Run(() => Get(name, password));
        }

        public long RowCount()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<IUser>().RowCountInt64();
            }
        }

        public Task<long> RowCountAsync()
        {
            return Task.Run(() => RowCount());
        }

        public void Save(IUser user)
        {
            if (!CheckObject(user))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }
        }

        public Task SaveAsync(IUser user)
        {
            return Task.Run(() => Save(user));
        }

        public void Update(IUser user)
        {
            if (!CheckObject(user))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(user);
                transaction.Commit();
            }
        }

        public Task UpdateAsync(IUser user)
        {
            return Task.Run(() => Update(user));
        }

        public bool CheckObject(IUser user)
        {
            return user != null;
        }
    }
}
