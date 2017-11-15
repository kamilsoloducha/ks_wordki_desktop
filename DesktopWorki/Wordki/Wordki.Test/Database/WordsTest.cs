using NHibernate;
using NUnit.Framework;
using Repository.Models;
using System.Linq;
using Wordki.Database;
using Wordki.Models;

namespace Wordki.Test.Database
{
    [TestFixture]
    public class WordTests
    {

        private IWord word;
        private IGroup group;
        private IGroupRepository groupRepo;
        private IWordRepository wordRepo;

        [SetUp]
        public void SetUp()
        {
            NHibernateHelper.ResetSession();
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
            wordRepo = new WordRepository();
        }

        [Test]
        public void Save_word_by_save_group_test()
        {
            groupRepo.Save(group);
            group.Words.Add(word);
            word.Group = group;
            Assert.Throws<NHibernate.Exceptions.GenericADOException>(() => groupRepo.Save(group));
        }

        [Test]
        public void Save_word_by_groupRepo_test()
        {
            groupRepo.Save(group);
            group.Words.Add(word);
            word.Group = group;
            groupRepo.Update(group);

            IGroup groupFromDatabase = groupRepo.Get(group.Id);

            Assert.AreEqual(1, groupFromDatabase.Words.Count);
        }

        [Test]
        public void Save_word_with_group_at_once_test()
        {
            group.Words.Add(word);
            word.Group = group;
            groupRepo.Save(group);

            IGroup groupFromDatabase = groupRepo.Get(group.Id);

            Assert.AreEqual(1, groupFromDatabase.Words.Count);
        }

        [Test]
        public void Save_word_by_word_repository_test()
        {
            groupRepo.Save(group);
            word.Group = group;
            wordRepo.Save(word);

            IGroup groupFromDatabase = groupRepo.Get(group.Id);

            Assert.AreEqual(1, groupFromDatabase.Words.Count, $"Word is not added");

            IWord wordFromDatabase = groupFromDatabase.Words[0];
            Assert.AreEqual(word.Group.Id, wordFromDatabase.Group.Id, $"Wrong groupId after read from database");
        }

        [Test]
        public void Save_word_without_group_test()
        {
            Assert.Throws<PropertyValueException>(() => wordRepo.Save(word));
        }

        [Test]
        public void Get_word_by_groupRepo_test()
        {
            group.Words.Add(word);
            word.Group = group;
            groupRepo.Save(group);
            IWord wordFromDatabase = groupRepo.Get(group.Id).Words.First();
            Assert.AreEqual(word, wordFromDatabase);
        }

        [Test]
        public void Get_word_by_wordRepo_test()
        {
            group.Words.Add(word);
            word.Group = group;
            groupRepo.Save(group);
            IWord wordFromDatabase = wordRepo.Get(word.Id);
            CheckWordEquality(word, wordFromDatabase);
        }

        [Test]
        public void Update_word_by_groupRepo_test()
        {
            group.Words.Add(word);
            word.Group = group;
            groupRepo.Save(group);
            ChangeWord(word);
            groupRepo.Update(group);
            IWord wordFromDatabase = wordRepo.Get(word.Id);
            CheckWordEquality(word, wordFromDatabase);
        }

        [Test]
        public void Update_word_by_wordRepo_test()
        {
            group.Words.Add(word);
            word.Group = group;
            groupRepo.Save(group);
            ChangeWord(word);
            wordRepo.Update(word);
            IWord wordFromDatabase = wordRepo.Get(word.Id);
            CheckWordEquality(word, wordFromDatabase);
        }

        [Test]
        public void Delete_word_by_wordRepo_test()
        {
            group.Words.Add(word);
            word.Group = group;
            groupRepo.Save(group);
            wordRepo.Delete(word);
            Assert.AreEqual(0, wordRepo.RowCount());
        }

        [Test]
        public void Delete_word_by_groupRepo_test()
        {
            group.Words.Add(word);
            word.Group = group;
            groupRepo.Save(group);
            groupRepo.Delete(group);
            Assert.AreEqual(0, wordRepo.RowCount());
        }



        [TearDown]
        public void TearDown()
        {
            NHibernateHelper.ClearDatabase();
        }

        private void ChangeWord(IWord word)
        {
            word.Language1 = "fdsa";
            word.Language2 = "fdsa";
            word.Language1Comment = "fdsa";
            word.Language2Comment = "fdsa";
            word.Drawer = 0;
        }

        private void CheckWordEquality(IWord wordExpected, IWord wordActual)
        {
            Assert.NotNull(wordExpected.Group, "Group in word is null");
            Assert.NotNull(wordActual.Group, "Group in word is null");
            Assert.AreEqual(wordExpected, wordActual, $"Words are not equal");
        }
    }
}
