using NUnit.Framework;

using System.Collections.Generic;
using Wordki.Database;
using Wordki.Database.Repositories;
using WordkiModel;

namespace Wordki.Test.Database
{
    [TestFixture]
    public class NHibernateHelperTests
    {

        private IUserRepository userRepo = new UserRepository();
        private IGroupRepository groupRepo = new GroupRepository();
        private IWordRepository wordRepo = new WordRepository();
        private IResultRepository resultRepo = new ResultRepository();
        

        [SetUp]
        public void SetUp()
        {
            NHibernateHelper.ResetSession();
            NHibernateHelper.ClearDatabase();
        }


        [Test]
        public void Clean_all_database_test()
        {
            userRepo.Save(Utility.GetUser());
            groupRepo.Save(Utility.GetGroups());

            NHibernateHelper.ClearDatabase();

            Assert.AreEqual(0, userRepo.RowCount(), "Error in user number in database");
            Assert.AreEqual(0, groupRepo.RowCount(), "Error in group number in database");
            Assert.AreEqual(0, wordRepo.RowCount(), "Error in word number in database");
            Assert.AreEqual(0, resultRepo.RowCount(), "Error in result number in database");
        }

        [Test]
        public void Refresh_database_all_removed_test()
        {
            IEnumerable<IGroup> groups = Utility.GetGroups();
            foreach(IGroup group in groups)
            {
                group.State = -1;
                foreach (IWord word in group.Words)
                {
                    word.State = -1;
                }
                foreach (IResult result in group.Results)
                {
                    result.State = -1;
                }
            }
            groupRepo.Save(groups);

            NHibernateHelper.RefreshDatabase();

            Assert.AreEqual(0, groupRepo.RowCount(), "Error in group number in database");
            Assert.AreEqual(0, wordRepo.RowCount(), "Error in word number in database");
            Assert.AreEqual(0, resultRepo.RowCount(), "Error in result number in database");
        }

        [Test]
        public void Refresh_database_all_updated_check_count_test()
        {
            IEnumerable<IGroup> groups = Utility.GetGroups();
            foreach (IGroup group in groups)
            {
                group.State = 1;
                foreach (IWord word in group.Words)
                {
                    word.State = 1;
                }
                foreach (IResult result in group.Results)
                {
                    result.State = 1;
                }
            }
            groupRepo.Save(groups);

            NHibernateHelper.RefreshDatabase();

            Assert.AreEqual(Utility.GroupCount, groupRepo.RowCount(), "Error in group number in database");
            Assert.AreEqual(Utility.GroupCount * Utility.WordCount, wordRepo.RowCount(), "Error in word number in database");
            Assert.AreEqual(Utility.GroupCount * Utility.ResultCount, resultRepo.RowCount(), "Error in result number in database");
        }

        [Test]
        public void Refresh_database_all_updated_check_state_test()
        {
            IEnumerable<IGroup> groups = Utility.GetGroups();
            foreach (IGroup group in groups)
            {
                group.State = 1;
                foreach (IWord word in group.Words)
                {
                    word.State = 1;
                }
                foreach (IResult result in group.Results)
                {
                    result.State = 1;
                }
            }
            groupRepo.Save(groups);

            NHibernateHelper.RefreshDatabase();

            foreach(IGroup group in groupRepo.GetAll())
            {
                Assert.AreEqual(0, group.State);
                foreach(IWord word in group.Words)
                {
                    Assert.AreEqual(0, word.State);
                }
                foreach(IResult result in group.Results)
                {
                    Assert.AreEqual(0, result.State);
                }
            }
        }


        [TearDown]
        public void TearDown()
        {
            NHibernateHelper.ClearDatabase();
        }

    }
}
