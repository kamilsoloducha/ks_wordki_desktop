using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Models.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Helpers.GroupSplitter;
using Wordki.Models;

namespace Wordki.Test.Helpers.GroupSplitterTests
{
    [TestClass]
    public class GroupSplitterBaseTest
    {

        static GroupSplitterBase splitter;

#if TEST
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            splitter = new GroupSlitPercentage();
        }

        [TestMethod]
        public void CreateGroupTest()
        {
            string name = "Nazwa";
            int counter = 10;
            Group group = new Group() { Name = name, Language1 = LanguageFactory.GetLanguage(LanguageType.English), Language2 = LanguageFactory.GetLanguage(LanguageType.English) };
            Group newGroup = splitter.CreateGroupTest(group, counter);
            Assert.IsTrue(newGroup.Name.Equals($"{name} {new string('*', counter)}"));

        }

#endif

    }
}
