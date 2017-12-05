using NUnit.Framework;
using Repository.Models;
using System.Linq;
using System.Collections.Generic;
using Wordki.Database;
using Wordki.Models;
using Wordki.Database.Repositories;

namespace Wordki.Test.Database
{
    [TestFixture]
    public class AdvanceGroupTests
    {
        private IGroupRepository groupRepo;
        private IWordRepository wordRepo;
        private IResultRepository resultRepo;

        [SetUp]
        public void SetUp()
        {
            NHibernateHelper.ResetSession();
            NHibernateHelper.ClearDatabase();
            groupRepo = new GroupRepository();
            wordRepo = new WordRepository();
            resultRepo = new ResultRepository();
        }

        [Test, Order(0)]
        public void Save_group_check_count_test()
        {
            IGroup group = Utility.GetGroup();
            groupRepo.Save(group);

            Assert.AreEqual(1, groupRepo.RowCount(), "Error in row count");
        }

        [Test, Order(10)]
        public void Save_groups_check_count_test()
        {
            IList<IGroup> groups = Utility.GetGroups();
            groupRepo.Save(groups);

            Assert.AreEqual(Utility.GroupCount, groupRepo.RowCount(), "Error in row count");
        }

        [Test, Order(20)]
        public void Save_groups_check_words_test()
        {
            IList<IGroup> groups = Utility.GetGroups();
            groupRepo.Save(groups);

            Assert.AreEqual(Utility.GroupCount * Utility.WordCount, wordRepo.RowCount(), "Error in row count");
        }

        [Test, Order(30)]
        public void Save_groups_check_results_test()
        {
            IList<IGroup> groups = Utility.GetGroups();
            groupRepo.Save(groups);

            Assert.AreEqual(Utility.GroupCount * Utility.ResultCount, resultRepo.RowCount(), "Error in row count");
        }

        [Test, Order(40)]
        public void Update_group_and_check_test()
        {
            IList<IGroup> groups = Utility.GetGroups();
            groupRepo.Save(groups);

            IGroup groupToChange = groups.First();
            groupToChange.Name = "asdf";
            groupToChange.Language1 = Repository.Models.Language.LanguageType.Portuaglese;
            groupToChange.Language2 = Repository.Models.Language.LanguageType.Portuaglese;

            groupRepo.Update(groupToChange);
            IGroup groupFromDatabase = groupRepo.Get(groupToChange.Id);

            Assert.AreEqual(groupToChange, groupFromDatabase, "Groups are not the same");
        }

        [Test, Order(50)]
        public void Update_word_from_groupRepo_test()
        {
            IList<IGroup> groups = Utility.GetGroups();
            groupRepo.Save(groups);

            IWord wordToChange = groups.First().Words.First();
            wordToChange.Language1 = "asdf";
            wordToChange.Language2 = "asdf";
            wordToChange.Language1Comment = "asdf";
            wordToChange.Language2Comment = "asdf";
            wordToChange.Drawer = 1;

            groupRepo.Update(groups);
            IWord wordFromDatabase = wordRepo.Get(wordToChange.Id);
            Assert.AreEqual(wordToChange, wordFromDatabase, "Word are not the same");            
        }

        [Test, Order(60)]
        public void Update_result_from_groupRepo_test()
        {
            IList<IGroup> groups = Utility.GetGroups();
            groupRepo.Save(groups);

            IResult resultToChange = groups.First().Results.First();
            resultToChange.Correct = 1;
            resultToChange.Wrong = 1;
            resultToChange.Accepted = 1;
            resultToChange.Invisibilities = 1;
            resultToChange.TranslationDirection = Repository.Models.Enums.TranslationDirection.FromFirst;
            resultToChange.State = -1;

            groupRepo.Update(groups);
            IResult resultFromDatabase = resultRepo.Get(resultToChange.Id);
            Assert.AreEqual(resultToChange, resultToChange, "Word are not the same");
        }

        [TearDown]
        public void TearDown()
        {
            NHibernateHelper.ClearDatabase();
        }

    }
}
