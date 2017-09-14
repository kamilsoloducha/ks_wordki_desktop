using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class GroupSplitterGroupCountTest
    {
        static GroupSplitterBase splitter;
        Group group;
        int wordCount = 100;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            splitter = new GroupSplitGroupCount();
        }

        [TestInitialize]
        public void Init()
        {
            group = new Group();
            for (int i = 0; i < wordCount; i++)
            {
                group.WordsList.Add(new Word());
            }
        }

        [TestMethod]
        public void Try_split_empty_group_test()
        {
            Group g = new Group();
            IEnumerable<Group> result = splitter.Split(g, 1);
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void Try_split_custom_group_test()
        {
            int factor = 10;
            IEnumerable<Group> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == factor - 1);
            foreach (Group g in result)
            {
                Assert.IsTrue(g.WordsList.Count == wordCount / factor);
            }
        }

        [TestMethod]
        public void Try_split_custom_group_on_not_equal_test()
        {
            int factor = 11;
            IEnumerable<Group> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == factor - 1);
            foreach (Group g in result)
            {
                Assert.IsTrue(g.WordsList.Count == wordCount / factor || g.WordsList.Count == wordCount / factor -1);
            }
        }

        [TestMethod]
        public void Try_split_group_with_exteme_factor_test()
        {
            IEnumerable<Group> result = splitter.Split(group, 0);
            Assert.IsTrue(result.Count() == 0);

            result = splitter.Split(group, 1);
            Assert.IsTrue(result.Count() == 0);

            result = splitter.Split(group, result.Count());
            Assert.IsTrue(result.Count() == 0);
        }

    }
}
