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
    public class GroupSplitterWordCountTest
    {

        static GroupSplitterBase splitter;
        Group group;
        int wordCount = 100;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            splitter = new GroupSplitWordCount();
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
            IEnumerable<Group> result = splitter.Split(g, 10);
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void Try_split_custom_group()
        {
            int factor = 10;
            IEnumerable<Group> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == group.WordsList.Count / factor - 1);
            foreach(var g in result)
            {
                Assert.IsTrue(g.WordsList.Count() == 10);
            }
        }

        [TestMethod]
        public void Try_split_custom_group_with_odd_factor()
        {
            int factor = 9;
            IEnumerable<Group> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == group.WordsList.Count / factor - 1);
            foreach (var g in result)
            {
                Assert.IsTrue(g.WordsList.Count() == group.WordsList.Count / factor || g.WordsList.Count() == group.WordsList.Count / factor + 1);
            }
        }

    }
}
