using Moq;
using NUnit.Framework;
using Wordki.Commands;
using Wordki.Database;
using WordkiModel;

namespace Wordki.Test.Commands.AddGroupActionTests
{
    [TestFixture]
    public class AddGroupActionTest
    {

        AddGroupAction action;
        Mock<IGroupSelectable> groupSelectableMock;
        Mock<IDatabase> databaseMock;

        [SetUp]
        public void SetUp()
        {
            groupSelectableMock = new Mock<IGroupSelectable>();
            databaseMock = new Mock<IDatabase>();
        }

        [Test]
        public void Add_group_if_selected_is_null()
        {
            groupSelectableMock.Setup(x => x.SelectedGroup).Returns(null as IGroup);
            action = new AddGroupAction(groupSelectableMock.Object, databaseMock.Object);
            action.Action();
            databaseMock.Verify(x => x.AddGroupAsync(
                It.Is<IGroup>(y => y.Language1 == Oazachaosu.Core.Common.LanguageType.Default && y.Language2 == Oazachaosu.Core.Common.LanguageType.Default)), Times.Once());
        }

        [Test]
        public void Add_group_if_selected_is_not_null()
        {
            IGroup group = Utility.GetGroup();
            groupSelectableMock.Setup(x => x.SelectedGroup).Returns(group);
            action = new AddGroupAction(groupSelectableMock.Object, databaseMock.Object);
            action.Action();
            databaseMock.Verify(x => x.AddGroupAsync(
                It.Is<IGroup>(y => y.Language1 == group.Language1 && y.Language2 == group.Language2)), Times.Once());
        }

    }
}
