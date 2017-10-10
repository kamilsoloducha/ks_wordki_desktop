using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Database2;
using Wordki.Models;

namespace Wordki.Test.Database
{
    [TestFixture]
    public class WordsTest
    {


        [SetUp]
        public void CreateSchema()
        {
            DeleteDatabaseIfExists();

            var schemaUpdate = new SchemaUpdate(NHibernateHelper.Configuration);
            schemaUpdate.Execute(false, true);
        }

        [Test]
        public void Test()
        {
            Group group = new Group();
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(group);
                transaction.Commit();
            }
        }


        public void DeleteDatabaseIfExists()
        {
            if (File.Exists(NHibernateHelper.DatabasePath))
                File.Delete(NHibernateHelper.DatabasePath);
        }
    }
}
