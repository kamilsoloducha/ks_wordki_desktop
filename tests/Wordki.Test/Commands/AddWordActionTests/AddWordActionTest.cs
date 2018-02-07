using Moq;
using NUnit.Framework;
using Wordki.Commands;
using Wordki.Database;
using WordkiModel;

namespace Wordki.Test.Commands.AddWordActionTests
{
    [TestFixture]
    public class AddWordActionTest
    {

        AddWordAction action;
        Mock<IGroupSelectable> groupSelectableMock;
        Mock<IWordSelectable> wordSelectableMock;
        Mock<IDatabase> databaseMock;

        [SetUp]
        public void SetUp()
        {
            groupSelectableMock = new Mock<IGroupSelectable>();
            wordSelectableMock = new Mock<IWordSelectable>();
            databaseMock = new Mock<IDatabase>();
        }

        [Test]
        public void Add_word_if_selected_group_is_null()
        {
            groupSelectableMock.Setup(x => x.SelectedGroup).Returns(null as IGroup);
            groupSelectableMock.SetupProperty(x => x.SelectedGroup);

            wordSelectableMock.SetupProperty(x => x.SelectedWord);
            action = new AddWordAction(groupSelectableMock.Object, wordSelectableMock.Object, databaseMock.Object);
            action.Action();
            databaseMock.Verify(x => x.AddGroupAsync(
                It.Is<IGroup>(y => y.Language1 == Oazachaosu.Core.Common.LanguageType.Default && y.Language2 == Oazachaosu.Core.Common.LanguageType.Default)), Times.Once());
            Assert.NotNull(groupSelectableMock.Object.SelectedGroup);
            Assert.AreEqual(1, groupSelectableMock.Object.SelectedGroup.Words.Count);
            Assert.NotNull(wordSelectableMock.Object.SelectedWord);
        }

        [Test]
        public void Add_word_if_selected_is_not_null()
        {
            IGroup group = Utility.GetGroup();
            group.Words.Clear();
            groupSelectableMock.Setup(x => x.SelectedGroup).Returns(group);

            wordSelectableMock.SetupProperty(x => x.SelectedWord);
            action = new AddWordAction(groupSelectableMock.Object, wordSelectableMock.Object, databaseMock.Object);
            action.Action();
            Assert.AreEqual(1, groupSelectableMock.Object.SelectedGroup.Words.Count);
            Assert.NotNull(wordSelectableMock.Object.SelectedWord);
        }

    }
}
