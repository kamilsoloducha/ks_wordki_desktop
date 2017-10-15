using NUnit.Framework;
using Repository.Models;
using System.IO;
using Wordki.Database2;

namespace Wordki.Test.Database.UserOrganizerTests
{
    [TestFixture]
    public class UserOrganizerTests
    {

        IUserOrganizer userOrganizer;
        IUser user;
        IDatabase database;
        Utility util = new Utility();

        [SetUp]
        public void SetUp()
        {
            userOrganizer = new UserOrganizer();
            user = util.GetUser();
            database = DatabaseSingleton.GetDatabase();
        }

        [Test]
        public void Create_database_with_user_test()
        {
            userOrganizer.AddDatabase(user);
            Assert.IsTrue(File.Exists(NHibernateHelper.DatabasePath), "Error in create Database file");

            IUser userFromDatabase = database.GetUserAsync(user.Name, user.Password).Result;

            Assert.IsNotNull(userFromDatabase, "Error in add user to Database");
            util.CheckUser(user, userFromDatabase);
        }

        [Test]
        public void Delete_database_with_correct_user_test()
        {
            userOrganizer.AddDatabase(user);
            userOrganizer.RemoveDatabase(user);
            Assert.IsTrue(!File.Exists(NHibernateHelper.DatabasePath), "Error in delete Database file");
        }

        [Test]
        public void Delete_database_with_wrong_user_test()
        {
            userOrganizer.AddDatabase(user);
            user.Password = "fdsa";
            userOrganizer.RemoveDatabase(user);
            Assert.IsTrue(File.Exists(NHibernateHelper.DatabasePath), "Error in delete Database file");
        }


        [TearDown]
        public void TearDown()
        {
            Directory.Delete(NHibernateHelper.DirectoryPath, true);
        }
    }
}
