using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Commands;

namespace Wordki.Test.Commands.SetFocusActionTests
{
    [TestFixture]
    public class SetFocusActionTest
    {

        Mock<IFocusSelectable> focusSelectableMock;
        SetFocusAction action;

        [SetUp]
        public void Setup()
        {
            focusSelectableMock = new Mock<IFocusSelectable>();
            action = new SetFocusAction(focusSelectableMock.Object, 0);
        }

        [Test]
        public void Try_to_change_focus()
        {
            ObservableCollection<bool> collection = new ObservableCollection<bool>() { false, false, true };
            focusSelectableMock.Setup(x => x.Focusable).Returns(collection);
            action.Action();

            Assert.IsTrue(focusSelectableMock.Object.Focusable[0]);
            Assert.IsFalse(focusSelectableMock.Object.Focusable[2]);

        }

    }
}
