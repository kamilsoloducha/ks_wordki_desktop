using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using Wordki.Models;
using Repository.Models;
using System.Threading.Tasks;

namespace Wordki.Database
{
    public class WordRepository : IWordRepository
    {
        public void Delete(IWord word)
        {
            if (!CheckObject(word))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(word);
                transaction.Commit();
            }
        }

        public Task DeleteAsync(IWord word)
        {
            return Task.Run(() => Delete(word));
        }

        public IWord Get(long id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Get<IWord>(id);
            }
        }

        public Task<IWord> GetAsync(long id)
        {
            return Task.Run(() => Get(id));
        }

        public IEnumerable<IWord> GetWords()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Query<IWord>().ToList();
            }
        }

        public Task<IEnumerable<IWord>> GetWordsAsync()
        {
            return Task.Run(() => GetWords());
        }

        public long RowCount()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<IWord>().RowCountInt64();
            }
        }

        public Task<long> RowCountAsync()
        {
            return Task.Run(() => RowCount());
        }

        public void Save(IWord word)
        {
            if (!CheckObject(word))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(word);
                transaction.Commit();
            }
        }

        public Task SaveAsync(IWord word)
        {
            return Task.Run(() => Save(word));
        }

        public void Update(IWord word)
        {
            if (!CheckObject(word))
            {
                return;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(word);
                transaction.Commit();
            }
        }

        public Task UpdateAsync(IWord word)
        {
            return Task.Run(() => Update(word));
        }

        private bool CheckObject(IWord word)
        {
            return word != null
                && word.Id > 0
                && word.Group != FakeGroup.Group;
        }
    }
}
