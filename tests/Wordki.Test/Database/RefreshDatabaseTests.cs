using NUnit.Framework;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Database;
using Wordki.Database.Repositories;
using Wordki.Models;

namespace Wordki.Test.Database
{
    [TestFixture]
    public class RefreshDatabaseTests
    {
        const int wordCount = 10;
        const int resultCount = 10;
        const int groupCount = 10;
        IDatabase database;
        Utility util = new Utility();
        IList<IGroup> groups;

        [SetUp]
        public void SetUp()
        {
            database = DatabaseSingleton.Instance;
            NHibernateHelper.ResetSession();
            NHibernateHelper.ClearDatabase();
            groups = util.GetGroups(groupCount);
            foreach(IGroup group in groups)
            {
                database.AddGroup(group);
            }
        }

        [TearDown]
        public void TearDown()
        {
            NHibernateHelper.ClearDatabase();
        }

        [Test]
        public void Refresh_database_with_deleted_whole_group_test()
        {
            RemoveWholeGroup();
            IGroupRepository groupRepo = new GroupRepository();
            Assert.AreEqual(groupCount - 1, groupRepo.RowCount());
        }

        [Test]
        public void Refresh_database_with_deleted_whole_group_check_words_count_test()
        {
            RemoveWholeGroup();
            IWordRepository repo = new WordRepository();
            Assert.AreEqual(wordCount * (groupCount -1), repo.RowCount());
        }

        [Test]
        public void Refresh_database_with_deleted_whole_group_check_results_count_test()
        {
            RemoveWholeGroup();
            IResultRepository repo = new ResultRepository();
            Assert.AreEqual(resultCount * (groupCount - 1), repo.RowCount());
        }

        [Test]
        public void Refresh_database_with_deleted_word_check_groups_count_test()
        {
            RemoveWord();
            IGroupRepository groupRepo = new GroupRepository();
            Assert.AreEqual(groupCount, groupRepo.RowCount());
        }

        [Test]
        public void Refresh_database_with_deleted_word_check_words_count_test()
        {
            RemoveWord();
            IWordRepository repo = new WordRepository();
            Assert.AreEqual(wordCount * groupCount - 1, repo.RowCount());
        }

        [Test]
        public void Refresh_database_with_deleted_word_check_results_count_test()
        {
            RemoveWord();
            IResultRepository repo = new ResultRepository();
            Assert.AreEqual(resultCount * groupCount, repo.RowCount());
        }

        [Test]
        public void Refresh_database_with_deleted_result_check_groups_count_test()
        {
            RemoveResult();
            IGroupRepository groupRepo = new GroupRepository();
            Assert.AreEqual(groupCount, groupRepo.RowCount());
        }

        [Test]
        public void Refresh_database_with_deleted_result_check_words_count_test()
        {
            RemoveResult();
            IWordRepository repo = new WordRepository();
            Assert.AreEqual(wordCount * groupCount, repo.RowCount());
        }

        [Test]
        public void Refresh_database_with_deleted_result_check_results_count_test()
        {
            RemoveResult();
            IResultRepository repo = new ResultRepository();
            Assert.AreEqual(resultCount * groupCount - 1, repo.RowCount());
        }

        private void RemoveWholeGroup()
        {
            database.DeleteGroup(groups[0]);
            database.RefreshDatabase();
        }

        private void RemoveWord()
        {
            database.DeleteWord(groups[0].Words[0]);
            database.RefreshDatabase();
        }
        private void RemoveResult()
        {
            database.DeleteResult(groups[0].Results[0]);
            database.RefreshDatabase();
        }


    }
}
