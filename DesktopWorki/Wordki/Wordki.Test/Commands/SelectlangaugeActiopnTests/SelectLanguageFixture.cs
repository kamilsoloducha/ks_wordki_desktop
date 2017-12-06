using NUnit.Framework;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Commands;

namespace Wordki.Test.Commands.SelectlangaugeActiopnTests
{
    [TestFixture]
    public class SelectLanguageFixture
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
            Assert.Throws<ArgumentNullException>(() => action.Action(null, 1, Repository.Models.Language.LanguageType.English));
        }

        [Test]
        public void Try_change_language_with_wrong_number_of_language_test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => action.Action(Utility.GetGroup(), 3, Repository.Models.Language.LanguageType.English));
        }

        [Test]
        public void Try_change_language1_in_group_test()
        {
            IGroup group = Utility.GetGroup(language1: Repository.Models.Language.LanguageType.English);
            action.Action(group, 1, Repository.Models.Language.LanguageType.French);
            Assert.AreEqual(Repository.Models.Language.LanguageType.French, group.Language1);
        }

        [Test]
        public void Try_change_language2_in_group_test()
        {
            IGroup group = Utility.GetGroup(language2: Repository.Models.Language.LanguageType.English);
            action.Action(group, 2, Repository.Models.Language.LanguageType.French);
            Assert.AreEqual(Repository.Models.Language.LanguageType.French, group.Language2);
        }

    }
}
