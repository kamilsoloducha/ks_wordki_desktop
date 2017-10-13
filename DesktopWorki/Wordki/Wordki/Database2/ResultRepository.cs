using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System.Collections.Generic;
using Wordki.Models;
using Repository.Models;

namespace Wordki.Database2
{
    public class ResultRepository : IResultRepository
    {
        public void Delete(IResult result)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(result as Result);
                transaction.Commit();
            }
        }

        public IResult Get(long id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Get<Result>(id);
            }
        }

        public IEnumerable<IResult> GetWords()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Query<Result>().ToList();
            }
        }

        public long RowCount()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Result>().RowCountInt64();
            }
        }

        public void Save(IResult result)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(result as Result);
                transaction.Commit();
            }
        }

        public void Update(IResult result)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(result as Result);
                transaction.Commit();
            }
        }
    }
}
