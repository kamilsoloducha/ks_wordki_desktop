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
    public class GroupSplitterPercentageTest
    {
        static GroupSplitterBase splitter;
        Group group;
        int wordCount = 100;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            splitter = new GroupSlitPercentage();
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
            int factor = 1;
            Group g = new Group();
            IEnumerable<Group> result = splitter.Split(g, factor);
            Assert.IsTrue(result.Count() == 0);
        }


        [TestMethod]
        public void Try_split_custom_group_on_half_test()
        {
            int factor = 50;
            IEnumerable<Group> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == 1);
            Group x = result.FirstOrDefault();
            Assert.IsTrue(x != null);
            Assert.IsTrue(x.WordsList.Count == 50);
        }

        [TestMethod]
        public void Try_split_group_with_100_factor_test()
        {
            int factor = 100;
            IEnumerable<Group> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void Try_split_group_with_0_factor_test()
        {
            int factor = 0;
            IEnumerable<Group> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void Try_split_group_with_random_factor_test()
        {
            int factor = new Random().Next(1, 99);
            IEnumerable<Group> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == 1);
            Group x = result.FirstOrDefault();
            Assert.IsTrue(x != null);
            Assert.IsTrue(x.WordsList.Count == 100 - factor);
        }

        [TestMethod]
        public void Try_split_group_with_odd_words_count_test()
        {
            group.WordsList.RemoveAt(0);
            int factor = 50;
            IEnumerable<Group> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == 1);
            Group x = result.FirstOrDefault();
            Assert.IsTrue(x != null);
            Assert.IsTrue(x.WordsList.Count == 50);
        }

    }
}
