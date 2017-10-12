using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Wordki.Database2;
using Wordki.Models;

namespace Wordki.Test.Database
{
    [TestFixture]
    public class GroupTests
    {

        private Group group;
        private IGroupRepository repo;

        [SetUp]
        public void SetUp()
        {
            NHibernateHelper.ClearDatabase();
            group = new Group()
            {
                Id = 1,
                Name = "test",
                Language1 = Repository.Models.Language.LanguageType.English,
                Language2 = Repository.Models.Language.LanguageType.Germany,
                State = 1,
                UserId = 1,
            };

            repo = new GroupRepository();
        }

        [Test]
        public void Save_group_to_database_test()
        {
            repo.Save(group);

            Assert.AreEqual(1, repo.RowCount());
        }

        [Test]
        public void Get_group_from_database_test()
        {
            repo.Save(group);
            Group groupFromDatabase = repo.Get(group.Id);
            Assert.IsTrue(groupFromDatabase.Equals(group));
        }

        [Test]
        public void Delete_group_from_database_test()
        {
            repo.Save(group);
            repo.Delete(group);
            Assert.AreEqual(0, repo.RowCount());
        }

        [Test]
        public void Save_more_group_to_database_test()
        {
            int groupCount = 10;
            for (int i = 0; i < groupCount; i++)
            {
                group.Id = i;
                repo.Save(group);
            }
            Assert.AreEqual(groupCount, repo.RowCount());
        }

        [Test]
        public void Get_more_group_from_database_test()
        {
            int groupCount = 10;
            for (int i = 0; i < groupCount; i++)
            {
                group.Id = i;
                repo.Save(group);
            }
            IEnumerable<Group> groups = repo.GetGroups();
            Assert.AreEqual(groupCount, groups.Count());
        }

        [Test]
        public void Update_group_in_database_test()
        {
            repo.Save(group);
            group.Language1 = Repository.Models.Language.LanguageType.Spanish;
            group.Language2 = Repository.Models.Language.LanguageType.Russian;
            group.Name = "asdf";
            repo.Update(group);
            Group groupFromDatabase = repo.Get(group.Id);
            Assert.AreEqual(group, groupFromDatabase);

        }

        [TearDown]
        public void TearDown()
        {
            NHibernateHelper.ClearDatabase();
        }





    }
}
