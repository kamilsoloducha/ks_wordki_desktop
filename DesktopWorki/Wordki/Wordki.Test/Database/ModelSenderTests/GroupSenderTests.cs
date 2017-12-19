using Moq;
using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;
using Wordki.Database;
using Wordki.Database.Repositories;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Test.Database.ModelSenderTests
{
    [TestFixture]
    public class GroupSenderTests
    {

        private Mock<IGroupRepository> mock;
        private IEnumerable<IGroup> groups;
        private IModelSender<IGroup> modelSender;

        static GroupSenderTests()
        {
            Utility.ResultCount = 10;
            Utility.WordCount = 10;
            Utility.GroupCount = 10;
        }

        [SetUp]
        public void SetUp()
        {
            groups = Utility.GetGroups();
            mock = new Mock<IGroupRepository>();
        }

        [Test]
        public void Get_group_to_send_all_groups_not_to_send_test()
        {
            foreach (IGroup group in groups)
            {
                group.State = 0;
            }
            mock.Setup(m => m.GetAll()).Returns(groups);
            modelSender = new GroupsSender() { GroupRepo = mock.Object };

            Assert.AreEqual(groups.Count(x => x.State != 0), modelSender.GetModelToSend().Count());

        }

        [Test]
        public void Get_group_to_send_all_groups_to_send_test()
        {
            foreach (IGroup group in groups)
            {
                group.State = 1;
            }
            mock.Setup(m => m.GetAll()).Returns(groups);
            modelSender = new GroupsSender() { GroupRepo = mock.Object };

            Assert.AreEqual(groups.Count(x => x.State != 0), modelSender.GetModelToSend().Count());
        }

        [Test]
        public void Get_group_to_send_two_groups_to_send_test()
        {
            foreach (IGroup group in groups)
            {
                group.State = 0;
            }
            groups.First().State = 1;
            groups.Last().State = 1;
            mock.Setup(m => m.GetAll()).Returns(groups);
            modelSender = new GroupsSender() { GroupRepo = mock.Object };

            Assert.AreEqual(groups.Count(x => x.State != 0), modelSender.GetModelToSend().Count());
        }

    }
}
