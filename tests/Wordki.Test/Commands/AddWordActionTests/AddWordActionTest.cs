using Moq;
using NUnit.Framework;
using Wordki.Commands;
using Wordki.Database;
using WordkiModel;
using WordkiModel.Extensions;

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
            action = new AddWordAction(groupSelectableMock.Object, wordSelectableMock.Object, databaseMock.Object);
            action.Action();
            databaseMock.Verify(x => x.AddGroupAsync(
                It.Is<IGroup>(y => y.Language1 == Oazachaosu.Core.Common.LanguageType.Default && y.Language2 == Oazachaosu.Core.Common.LanguageType.Default)), Times.Once());
            groupSelectableMock.Verify(x => x.SelectedGroup.Words.Add(It.IsAny<IWord>()), Times.Once());
            wordSelectableMock.VerifySet(x => x.SelectedWord = It.IsAny<IWord>(), Times.Once());
        }

        [Test]
        public void Add_group_if_selected_is_not_null()
        {
            IGroup group = Utility.GetGroup();
            groupSelectableMock.Setup(x => x.SelectedGroup).Returns(group);
            action = new AddWordAction(groupSelectableMock.Object, wordSelectableMock.Object, databaseMock.Object);
            action.Action();
            databaseMock.Verify(x => x.AddGroupAsync(
                It.Is<IGroup>(y => y.Language1 == group.Language1 && y.Language2 == group.Language2)), Times.Once());
        }

    }
}
