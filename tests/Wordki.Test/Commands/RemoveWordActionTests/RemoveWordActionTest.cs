using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Wordki.Commands;
using Wordki.Database;
using WordkiModel;

namespace Wordki.Test.Commands.RemoveWordActionTests
{
    [TestFixture]
    public class RemoveWordActionTest
    {

        RemoveWordAction action;
        Mock<IGroupSelectable> groupSelectableMock;
        Mock<IWordSelectable> wordSelectableMock;
        Mock<IDatabase> databaseMock;

        [SetUp]
        public void SetUp()
        {
            groupSelectableMock = new Mock<IGroupSelectable>();
            wordSelectableMock = new Mock<IWordSelectable>();
            databaseMock = new Mock<IDatabase>();
            action = new RemoveWordAction(groupSelectableMock.Object, wordSelectableMock.Object, databaseMock.Object);
        }

        [Test]
        public void Remove_word_if_selected_word_is_null_and_selected_group_is_null()
        {
            groupSelectableMock.SetupProperty(x => x.SelectedGroup, null);
            wordSelectableMock.SetupProperty(x => x.SelectedWord, null);
            action.Action();
        }

        [Test]
        public void Remove_word_if_selected_word_is_null()
        {
            IList<IGroup> groups = Utility.GetGroups(1);
            groupSelectableMock.SetupProperty(x => x.SelectedGroup, groups[0]);
            wordSelectableMock.SetupProperty(x => x.SelectedWord, null);
            databaseMock.Setup(x => x.Groups).Returns(groups);
            action.Action();

            databaseMock.Verify(x => x.DeleteGroupAsync(groups[0]), Times.Once());
        }

        [Test]
        public void Remove_word_if_selected_word_is_not_null()
        {
            IList<IGroup> groups = Utility.GetGroups(1);
            groupSelectableMock.SetupProperty(x => x.SelectedGroup, groups[0]);
            wordSelectableMock.SetupProperty(x => x.SelectedWord, groups[0].Words[2]);
            databaseMock.Setup(x => x.Groups).Returns(groups);
            action.Action();

            Assert.AreSame(groups[0].Words[3], wordSelectableMock.Object.SelectedWord);
            

        }

    }
}
