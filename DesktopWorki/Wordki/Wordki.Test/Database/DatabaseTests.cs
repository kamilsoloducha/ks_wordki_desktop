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
    public class DatabaseTests
    {

        private IUserRepository _userRepo;

        [SetUp]
        public void CreateSchema()
        {
            DeleteDatabaseIfExists();

            var schemaUpdate = new SchemaUpdate(NHibernateHelper.Configuration);
            schemaUpdate.Execute(false, true);

            _userRepo = new UserRepository();
        }

        [Test]
        public void CanSavePerson()
        {
            _userRepo.Save(new User());
            Assert.AreEqual(1, _userRepo.RowCount());
        }

        [TearDown]
        public void DeleteDatabaseIfExists()
        {
            if (File.Exists(NHibernateHelper.DatabasePath))
                File.Delete(NHibernateHelper.DatabasePath);
        }
    }
}
