using Moq;
using NUnit.Framework;
using System.Linq;
using Wordki.Commands;
using WordkiModel;

namespace Wordki.Test.Commands.SelectPreviousWordActionTests
{
    [TestFixture]
    public class SelectPreviousWordActionTest
    {

        SelectPreviousWordAction action;
        Mock<IGroupSelectable> mockGroupSelectable;
        Mock<IWordSelectable> mockWordSelectable;


        [SetUp]
        public void SetUp()
        {
            mockGroupSelectable = new Mock<IGroupSelectable>();
            mockWordSelectable = new Mock<IWordSelectable>();
        }

        [Test]
        public void Select_next_word_if_selected_group_is_null()
        {
            mockGroupSelectable.Setup(x => x.SelectedGroup).Returns(null as IGroup);
            action = new SelectPreviousWordAction(mockGroupSelectable.Object, mockWordSelectable.Object);
            action.Action();
            mockWordSelectable.VerifySet(x => x.SelectedWord = It.IsAny<IWord>(), Times.Never());
        }

        [Test]
        public void Select_next_word_if_words_is_empty()
        {
            IGroup group = Utility.GetGroup();
            group.Words.Clear();
            mockGroupSelectable.Setup(x => x.SelectedGroup).Returns(group);
            action = new SelectPreviousWordAction(mockGroupSelectable.Object, mockWordSelectable.Object);
            action.Action();
            mockWordSelectable.VerifySet(x => x.SelectedWord = It.IsAny<IWord>(), Times.Never());
        }

        [Test]
        public void Select_next_word_if_selected_word_is_null()
        {
            IGroup group = Utility.GetGroup();
            mockGroupSelectable.Setup(x => x.SelectedGroup).Returns(group);
            mockWordSelectable.Setup(x => x.SelectedWord).Returns(null as IWord);
            action = new SelectPreviousWordAction(mockGroupSelectable.Object, mockWordSelectable.Object);
            action.Action();
            mockWordSelectable.VerifySet(x => x.SelectedWord = It.IsAny<IWord>(), Times.Never());
        }

        [Test]
        public void Select_next_word_if_selected_word_is_last()
        {
            IGroup group = Utility.GetGroup();
            mockGroupSelectable.Setup(x => x.SelectedGroup).Returns(group);
            mockWordSelectable.Setup(x => x.SelectedWord).Returns(group.Words.First());
            action = new SelectPreviousWordAction(mockGroupSelectable.Object, mockWordSelectable.Object);
            action.Action();
            mockWordSelectable.VerifySet(x => x.SelectedWord = It.IsAny<IWord>(), Times.Never());
        }

        [Test]
        public void Select_next_word_if_selected_word_is_custom()
        {
            IGroup group = Utility.GetGroup();
            mockGroupSelectable.Setup(x => x.SelectedGroup).Returns(group);
            mockWordSelectable.Setup(x => x.SelectedWord).Returns(group.Words[group.Words.Count / 2]);
            action = new SelectPreviousWordAction(mockGroupSelectable.Object, mockWordSelectable.Object);
            action.Action();
            mockWordSelectable.VerifySet(x => x.SelectedWord = group.Words[group.Words.Count / 2 - 1], Times.Once());
        }
    }

}
