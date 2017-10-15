using NUnit.Framework;
using Repository.Models;
using System;
using Wordki.Database2;
using Wordki.Models;

namespace Wordki.Test.Database
{
    [TestFixture]
    public class UserTests
    {

        private IUserRepository userRepo;
        private IUser user;
        private Utility util = new Utility();

        [SetUp]
        public void SetUp()
        {
            NHibernateHelper.ResetSession();
            NHibernateHelper.ClearDatabase();
            userRepo = new UserRepository();
            user = util.GetUser();
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
            util.CheckUser(user, userFromDatabase);
        }

        [Test]
        public void Update_user_test()
        {
            userRepo.Save(user);
            user.AllWords = false;
            user.DownloadTime = new DateTime(2000,1,1);
            user.Password = "fdsa";
            user.Timeout = 0;
            user.TranslationDirection = Repository.Models.Enums.TranslationDirection.FromFirst;
            userRepo.Update(user);
            IUser userFromDatabase = userRepo.Get(user.Name, user.Password);
            util.CheckUser(user, userFromDatabase);
        }

        [TearDown]
        public void TearDown()
        {
            NHibernateHelper.ClearDatabase();
        }

        

    }
}
