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
    public class WordTests
    {

        private Word word;
        private Group group;
        private IGroupRepository groupRepo;

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
            word = new Word()
            {
                Id = 1,
                Language1 = "word1",
                Language2 = "slowo1",
                Language1Comment = "test",
                Language2Comment = "test",
                Drawer = 4,
                State = 1,
                UserId = 1,
            };

            groupRepo = new GroupRepository();
        }

        [Test]
        public void Save_word_to_database_test()
        {
            groupRepo.Save(group);
            group.Words.Add(word);
            word.Group = group;
            groupRepo.Update(group);

            Group groupFromDatabase = groupRepo.Get(group.Id);

            Assert.AreEqual(1, groupFromDatabase.Words.Count);
        }

        [Test]
        public void Save_word_with_group_at_once_test()
        {
            group.Words.Add(word);
            word.Group = group;
            groupRepo.Save(group);

            Group groupFromDatabase = groupRepo.Get(group.Id);

            Assert.AreEqual(1, groupFromDatabase.Words.Count);
        }

        [Test]
        public void Get_word_from_database_test()
        {
            //repo.Save(word);
            Group groupFromDatabase = groupRepo.Get(word.Id);
            Assert.IsTrue(groupFromDatabase.Equals(word));
        }

        [Test]
        public void Delete_group_from_database_test()
        {
            //repo.Save(word);
            //repo.Delete(word);
            Assert.AreEqual(0, groupRepo.RowCount());
        }

        [Test]
        public void Save_more_group_to_database_test()
        {
            int groupCount = 10;
            for (int i = 0; i < groupCount; i++)
            {
                word.Id = i;
                //repo.Save(word);
            }
            Assert.AreEqual(groupCount, groupRepo.RowCount());
        }

        [Test]
        public void Get_more_group_from_database_test()
        {
            int groupCount = 10;
            for (int i = 0; i < groupCount; i++)
            {
                word.Id = i;
                //repo.Save(word);
            }
            IEnumerable<Group> groups = groupRepo.GetGroups();
            Assert.AreEqual(groupCount, groups.Count());
        }

        [Test]
        public void Update_group_in_database_test()
        {
            //repo.Save(word);
            //word.Language1 = Repository.Models.Language.LanguageType.Spanish;
            //word.Language2 = Repository.Models.Language.LanguageType.Russian;
            //word.Name = "asdf";
            //repo.Update(word);
            Group groupFromDatabase = groupRepo.Get(word.Id);
            Assert.AreEqual(word, groupFromDatabase);

        }

        [TearDown]
        public void TearDown()
        {
            NHibernateHelper.ClearDatabase();
        }
    }
}
