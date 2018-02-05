using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System.Collections.Generic;
using Wordki.Models;
using WordkiModel;
using System.Threading.Tasks;
using Wordki.Database.Repositories;

namespace Wordki.Database
{
    public class ResultRepository : IResultRepository
    {
        public void Delete(IResult result)
        {
            if (!CheckObject(result))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(result as Result);
                transaction.Commit();
            }
        }

        public Task DeleteAsync(IResult result)
        {
            return Task.Run(() => Delete(result));
        }

        public IResult Get(long id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Get<Result>(id);
            }
        }

        public Task<IResult> GetAsync(long id)
        {
            return Task.Run(() => Get(id));
        }

        public IEnumerable<IResult> GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Query<Result>().ToList();
            }
        }

        public Task<IEnumerable<IResult>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public long RowCount()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Result>().RowCountInt64();
            }
        }

        public Task<long> RowCountAsync()
        {
            return Task.Run(() => RowCount());
        }

        public void Save(IResult result)
        {
            if (!CheckObject(result))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(result as Result);
                transaction.Commit();
            }
        }

        public Task SaveAsync(IResult result)
        {
            return Task.Run(() => Save(result));
        }

        public void Update(IResult result)
        {
            if (!CheckObject(result))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(result as Result);
                transaction.Commit();
            }
        }

        public Task UpdateAsync(IResult result)
        {
            return Task.Run(() => Update(result));
        }

        public bool CheckObject(IResult result)
        {
            return result != null
                && result.Id > 0
                && result.Group != FakeGroup.Group;
        }

        public void Save(IEnumerable<IResult> items)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (IResult item in items)
                {
                    if (!CheckObject(item))
                    {
                        continue;
                    }
                    session.SaveOrUpdate(item);
                }
                transaction.Commit();
            }
        }

        public void Update(IEnumerable<IResult> items)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (IResult item in items)
                {
                    if (!CheckObject(item))
                    {
                        continue;
                    }
                    session.Update(item);
                }
                transaction.Commit();
            }
        }

        public Task SaveAsync(IEnumerable<IResult> items)
        {
            return Task.Run(() => Save(items));
        }

        public Task UpdateAsync(IEnumerable<IResult> items)
        {
            return Task.Run(() => Update(items));
        }
    }
}
