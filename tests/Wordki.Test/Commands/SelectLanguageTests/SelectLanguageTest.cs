using NUnit.Framework;
using Oazachaosu.Core.Common;
using System;
using Wordki.Commands;
using WordkiModel;

namespace Wordki.Test.Commands.SelectlangaugeActiopnTests
{
    [TestFixture]
    public class SelectLanguageTest
    {

        SelectLanguageAction action;

        [SetUp]
        public void SetUp()
        {
            action = ActionsSingleton<SelectLanguageAction>.Instance;
        }

        [Test]
        public void Try_change_language_with_null_group_test()
        {
            Assert.Throws<ArgumentNullException>(() => action.Action(null, 1, LanguageType.English));
        }

        [Test]
        public void Try_change_language_with_wrong_number_of_language_test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => action.Action(Utility.GetGroup(), 3, LanguageType.English));
        }

        [Test]
        public void Try_change_language1_in_group_test()
        {
            IGroup group = Utility.GetGroup(language1: LanguageType.English);
            action.Action(group, 1, LanguageType.French);
            Assert.AreEqual(LanguageType.French, group.Language1);
        }

        [Test]
        public void Try_change_language2_in_group_test()
        {
            IGroup group = Utility.GetGroup(language2: LanguageType.English);
            action.Action(group, 2, LanguageType.French);
            Assert.AreEqual(LanguageType.French, group.Language2);
        }

    }
}
