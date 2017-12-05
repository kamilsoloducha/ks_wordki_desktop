using NUnit.Framework;
using Repository.Models;
using System;
using System.IO;
using Wordki.Database;

namespace Wordki.Test.Database.UserOrganizerTests
{
    [TestFixture]
    public class DatabaseOrganizerTests
    {

        IDatabaseOrganizer databaseOrganizer;
        IUser user;
        IDatabase database;
        

        [SetUp]
        public void SetUp()
        {
            databaseOrganizer = new DatabaseOrganizer("");
            user = Utility.GetUser();
            database = DatabaseSingleton.Instance;
        }

        [Test]
        public void Create_database_with_user_test()
        {
            databaseOrganizer.AddDatabase(user);
            Assert.IsTrue(File.Exists(NHibernateHelper.DatabasePath), "Error in create Database file");

            IUser userFromDatabase = database.GetUserAsync(user.Name, user.Password).Result;

            Assert.IsNotNull(userFromDatabase, "Error in add user to Database");
            Utility.CheckUser(user, userFromDatabase);
        }

        [Test]
        public void Delete_database_with_correct_user_test()
        {
            databaseOrganizer.AddDatabase(user);
            databaseOrganizer.RemoveDatabase(user);
            Assert.IsTrue(!File.Exists(NHibernateHelper.DatabasePath), "Error in delete Database file");
        }

        [Test]
        public void Delete_database_with_wrong_user_test()
        {
            databaseOrganizer.AddDatabase(user);
            user.Password = "fdsa";
            databaseOrganizer.RemoveDatabase(user);
            Assert.IsTrue(File.Exists(NHibernateHelper.DatabasePath), "Error in delete Database file");
        }

        [Test]
        public void Try_to_get_databases_names_test()
        {
            databaseOrganizer.GetDatabases();
        }


        [TearDown]
        public void TearDown()
        {
            try
            {
                Directory.Delete(NHibernateHelper.DirectoryPath, true);
            }catch(Exception e)
            {

            }
        }
    }
}
