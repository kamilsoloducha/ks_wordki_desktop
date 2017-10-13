using NUnit.Framework;
using Repository.Models;
using System.Collections.Generic;
using System.Linq;
using Wordki.Helpers.GroupSplitter;
using Wordki.Models;

namespace Wordki.Test.Helpers.GroupSplitterTests
{
    [TestFixture]
    public class GroupSplitterWordCountTest
    {

        static GroupSplitterBase splitter;
        IGroup group;
        int wordCount = 100;

        [SetUp]
        public void Init()
        {
            splitter = new GroupSplitWordCount();
            group = new Group();
            for (int i = 0; i < wordCount; i++)
            {
                group.Words.Add(new Word() { Group = group, Id = i, });
            }
        }

        [Test]
        public void Try_split_empty_group_test()
        {
            IGroup g = new Group();
            IEnumerable<IGroup> result = splitter.Split(g, 10);
            Assert.IsTrue(result.Count() == 0);
        }

        [Test]
        public void Try_split_custom_group()
        {
            int factor = 10;
            IEnumerable<IGroup> result = splitter.Split(group, factor).ToList();
            Assert.AreEqual(wordCount / factor - 1, result.Count(), "Bad namber of groups");
            foreach (var g in result)
            {
                Assert.AreEqual(10, g.Words.Count(), "Bad number of words in list");
            }
        }

        [Test]
        public void Try_split_custom_group_with_odd_factor()
        {
            int factor = 9;
            IEnumerable<IGroup> result = splitter.Split(group, factor);
            Assert.AreEqual(wordCount / factor - 1, result.Count());
            foreach (var g in result)
            {
                Assert.IsTrue(g.Words.Count() >= wordCount / factor && g.Words.Count() < wordCount * 2 / factor);
            }
        }

    }
}
