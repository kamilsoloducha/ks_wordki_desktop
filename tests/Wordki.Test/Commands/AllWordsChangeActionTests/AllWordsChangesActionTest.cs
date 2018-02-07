using Moq;
using NUnit.Framework;
using Wordki.Commands;
using Wordki.Database;
using WordkiModel;

namespace Wordki.Test.Commands.AllWordsChangeActionTests
{

    [TestFixture]
    public class AllWordsChangesActionTest
    {

        Mock<IUserManager> userManagerMock;
        AllWordsChangeAction action;

        [SetUp]
        public void SetUp()
        {
            userManagerMock = new Mock<IUserManager>();
            action = new AllWordsChangeAction(userManagerMock.Object);
        }

        [Test]
        public void Try_to_change_all_words_parameter()
        {
            userManagerMock.SetupProperty(x => x.User.AllWords, true);
            action.Action();
            Assert.IsFalse(userManagerMock.Object.User.AllWords);
            userManagerMock.Verify(x => x.UpdateAsync(), Times.Once());
        }

    }
}
