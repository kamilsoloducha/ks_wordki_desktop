using NUnit.Framework;
using Oazachaosu.Core.Common;
using System;
using Wordki.Database;
using Wordki.Database.Repositories;
using WordkiModel;


namespace Wordki.Test.Database
{
    [TestFixture]
    public class UserTests
    {

        private IUserRepository userRepo;
        private IUser user;
        

        [SetUp]
        public void SetUp()
        {
            NHibernateHelper.ResetSession();
            NHibernateHelper.ClearDatabase();
            userRepo = new UserRepository();
            user = Utility.GetUser();
        }


        [Test]
        public void Save_user_to_database()
        {
            userRepo.Save(user);
            Assert.AreEqual(1, userRepo.RowCount());
        }

        [Test]
        public void Get_user_from_database()
        {
            userRepo.Save(user);
            IUser userFromDatabase = userRepo.Get(user.Name, user.Password);
            Utility.CheckUser(user, userFromDatabase);
        }

        [Test]
        public void Save_user_with_set_id()
        {
            user.Id = 10;
            userRepo.Save(user);
            IUser userFromDatabase = userRepo.Get(user.Name, user.Password);
            Assert.AreEqual(user.Id, userFromDatabase.Id);
        }

        [Test]
        public void Update_user_test()
        {
            userRepo.Save(user);
            user.AllWords = false;
            user.DownloadTime = new DateTime(2000,1,1);
            user.Password = "fdsa";
            user.TranslationDirection = TranslationDirection.FromFirst;
            userRepo.Update(user);
            IUser userFromDatabase = userRepo.Get(user.Name, user.Password);
            Utility.CheckUser(user, userFromDatabase);
        }

        [TearDown]
        public void TearDown()
        {
            NHibernateHelper.ClearDatabase();
        }

        

    }
}
