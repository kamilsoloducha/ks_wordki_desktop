using NUnit.Framework;
using System;
using Wordki.Commands;
using WordkiModel;

namespace Wordki.Test.Commands.CheckUncheckCommandTests
{
    [TestFixture]
    public class CheckUncheckCommandFixture
    {

        Action<IWord> action;

        [SetUp]
        public void SetUp()
        {
            action = ActionsSingleton<CheckUncheckAction>.Instance.Action;
        }

        [Test]
        public void Try_check_if_push_null_value_test()
        {
            action(null);
        }

        [Test]
        public void Try_change_value_from_true_test()
        {
            IWord word = Utility.GetWord(checkedUnchecked: true);
            action(word);
            Assert.AreEqual(false, word.Selected);
        }

        [Test]
        public void Try_change_value_from_false_test()
        {
            IWord word = Utility.GetWord(checkedUnchecked: false);
            action(word);
            Assert.AreEqual(true, word.Selected);
        }

    }
}
