using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Wordki.Commands;
using Wordki.Database;
using WordkiModel;

namespace Wordki.Test.Commands.SelectNextGroupActionTests
{
    [TestFixture]
    public class SelectNextGroupActionTest
    {

        SelectNextGroupAction action;
        List<IGroup> groups;
        Mock<IGroupSelectable> mockGroupSelectable;
        Mock<IDatabase> mockDatabase;

        [SetUp]
        public void SetUp()
        {
            groups = Utility.GetGroups(10);
            mockDatabase = new Mock<IDatabase>();
            mockDatabase.Setup(x => x.Groups).Returns(groups);
            mockGroupSelectable = new Mock<IGroupSelectable>();
        }

        [Test]
        public void Select_next_group_if_selected_is_null()
        {
            mockGroupSelectable.Setup(x => x.SelectedGroup).Returns(null as IGroup);
            IGroupSelectable groupSelectable = mockGroupSelectable.Object;
            action = new SelectNextGroupAction(groupSelectable, mockDatabase.Object);
            action.Action();
            mockGroupSelectable.VerifySet(m => m.SelectedGroup = It.IsAny<IGroup>(), Times.Never());
        }

        [Test]
        public void Select_next_group_if_selected_is_last()
        {
            mockGroupSelectable.Setup(x => x.SelectedGroup).Returns(groups.Last());
            IGroupSelectable groupSelectable = mockGroupSelectable.Object;
            action = new SelectNextGroupAction(groupSelectable, mockDatabase.Object);
            action.Action();
            mockGroupSelectable.VerifySet(m => m.SelectedGroup = It.IsAny<IGroup>(), Times.Never());
        }

        [Test]
        public void Select_next_group_if_selected_is_custom()
        {
            mockGroupSelectable.Setup(x => x.SelectedGroup).Returns(groups[5]);
            IGroupSelectable groupSelectable = mockGroupSelectable.Object;
            action = new SelectNextGroupAction(groupSelectable, mockDatabase.Object);
            action.Action();
            mockGroupSelectable.VerifySet(m => m.SelectedGroup = groups[6], Times.Once());
        }
    }
}
