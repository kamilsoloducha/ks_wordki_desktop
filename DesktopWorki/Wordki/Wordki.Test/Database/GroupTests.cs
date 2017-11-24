using NUnit.Framework;
using Repository.Models;
using System.Collections.Generic;
using System.Linq;
using Wordki.Database;
using Wordki.Database.Repositories;
using Wordki.Models;

namespace Wordki.Test.Database
{
    [TestFixture]
    public class GroupTests
    {

        private IGroup group;
        private IGroupRepository repo;
        private Utility util = new Utility() { ResultCount = 0, WordCount = 0, };

        [SetUp]
        public void SetUp()
        {
            NHibernateHelper.ResetSession();
            NHibernateHelper.ClearDatabase();
            group = util.GetGroup();

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
            IGroup groupFromDatabase = repo.Get(group.Id);
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
            IEnumerable<IGroup> groups = repo.GetAll();
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
            IGroup groupFromDatabase = repo.Get(group.Id);
            Assert.AreEqual(group, groupFromDatabase);
        }

        [Test]
        public void Update_the_same_group_test()
        {
            repo.Save(group);
            repo.Update(group);
        }

        [Test]
        public void Save_empty_group_test()
        {
            repo.Save(new Group());
        }


        [TearDown]
        public void TearDown()
        {
            NHibernateHelper.ClearDatabase();
        }





    }
}
