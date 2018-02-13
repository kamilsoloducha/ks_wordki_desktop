using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Util.Collections;
using Wordki.Commands;
using Wordki.Database;
using WordkiModel;

namespace Wordki.Test.Commands.RemoveGroupActionTests
{
    [TestFixture]
    public class RemoveGroupActionTest
    {

        RemoveGroupAction action;
        Mock<IGroupSelectable> groupSelectableMock;
        Mock<IWordSelectable> wordSelectableMock;
        Mock<IDatabase> databaseMock;

        [SetUp]
        public void SetUp()
        {
            groupSelectableMock = new Mock<IGroupSelectable>();
            wordSelectableMock = new Mock<IWordSelectable>();
            databaseMock = new Mock<IDatabase>();
            action = new RemoveGroupAction(groupSelectableMock.Object, wordSelectableMock.Object, databaseMock.Object);
        }

        [Test]
        public void Remove_group_if_selected_is_null()
        {
            groupSelectableMock.Setup(x => x.SelectedGroup).Returns(null as IGroup);
            action.Action();
        }

        [Test]
        public void Remove_group_if_selected_is_not_null()
        {
            IList<IGroup> groups = Utility.GetGroups(10);
            databaseMock.Setup(x => x.Groups).Returns(groups);
            groupSelectableMock.SetupProperty(x => x.SelectedGroup, groups[0]);
            wordSelectableMock.SetupProperty(x => x.SelectedWord, groups[0].Words[0]);
            action.Action();
            databaseMock.Verify(x => x.DeleteGroupAsync(groups[0]), Times.Once());
            IGroup expected = groups.Next(groups[0]);
            Assert.AreSame(expected, groupSelectableMock.Object.SelectedGroup);
            Assert.AreSame(expected.Words.LastOrDefault(), wordSelectableMock.Object.SelectedWord);
        }

        [Test]
        public void Remove_group_if_selected_is_last()
        {
            IList<IGroup> groups = Utility.GetGroups(10);
            databaseMock.Setup(x => x.Groups).Returns(groups);
            groupSelectableMock.SetupProperty(x => x.SelectedGroup, groups[9]);
            wordSelectableMock.SetupProperty(x => x.SelectedWord, groups[9].Words[0]);
            action.Action();
            databaseMock.Verify(x => x.DeleteGroupAsync(groups[9]), Times.Once());
            IGroup expected = groups.Previous(groups[9]);
            Assert.AreSame(expected, groupSelectableMock.Object.SelectedGroup);
            Assert.AreSame(expected.Words.LastOrDefault(), wordSelectableMock.Object.SelectedWord);
        }

        [Test]
        public void Remove_group_if_selected_is_last_one()
        {
            IList<IGroup> groups = Utility.GetGroups(1);
            databaseMock.Setup(x => x.Groups).Returns(groups);
            groupSelectableMock.SetupProperty(x => x.SelectedGroup, groups[0]);
            wordSelectableMock.SetupProperty(x => x.SelectedWord, groups[0].Words[0]);
            action.Action();
            databaseMock.Verify(x => x.DeleteGroupAsync(groups[0]), Times.Once());
            Assert.AreSame(null, groupSelectableMock.Object.SelectedGroup);
            Assert.AreSame(null, wordSelectableMock.Object.SelectedWord);
        }

    }
}
