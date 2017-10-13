using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using Wordki.Models;
using Repository.Models;

namespace Wordki.Database2
{
    public class WordRepository : IWordRepository
    {
        public void Delete(IWord word)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(word);
                transaction.Commit();
            }
        }

        public IWord Get(long id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Get<IWord>(id);
            }
        }

        public IEnumerable<IWord> GetWords()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Query<IWord>().ToList();
            }
        }

        public long RowCount()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<IWord>().RowCountInt64();
            }
        }

        public void Save(IWord word)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(word);
                transaction.Commit();
            }
        }

        public void Update(IWord word)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(word);
                transaction.Commit();
            }
        }
    }
}
