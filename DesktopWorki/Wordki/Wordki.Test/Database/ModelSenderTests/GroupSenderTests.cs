using Moq;
using NUnit.Framework;
using Repository.Models;
using System.Collections.Generic;
using System.Linq;
using Wordki.Database2;
using Wordki.Models;

namespace Wordki.Test.Database.ModelSenderTests
{
    [TestFixture]
    public class GroupSenderTests
    {

        private Mock<IGroupRepository> mock;
        private Utility util = new Utility() { WordCount = 0, ResultCount = 0, GroupCount = 5 };
        private IEnumerable<IGroup> groups;
        private IModelSender<IGroup> modelSender;

        [SetUp]
        public void SetUp()
        {
            groups = util.GetGroups();
            mock = new Mock<IGroupRepository>();
        }

        [Test]
        public void Get_group_to_send_all_groups_not_to_send_test()
        {
            foreach (IGroup group in groups)
            {
                group.State = 0;
            }
            mock.Setup(m => m.GetGroups()).Returns(groups);
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
            mock.Setup(m => m.GetGroups()).Returns(groups);
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
            mock.Setup(m => m.GetGroups()).Returns(groups);
            modelSender = new GroupsSender() { GroupRepo = mock.Object };

            Assert.AreEqual(groups.Count(x => x.State != 0), modelSender.GetModelToSend().Count());
        }

    }
}
