using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;
using Wordki.Helpers.GroupSplitter;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Test.Helpers.GroupSplitterTests
{
    [TestFixture]
    public class GroupSplitterGroupCountTest
    {
        static GroupSplitterBase splitter;
        IGroup group;
        int wordCount = 100;

        [SetUp]
        public void Init()
        {
            splitter = new GroupSplitGroupCount();
            group = new Group();
            for (int i = 0; i < wordCount; i++)
            {
                group.Words.Add(new Word() { Group = group, Id = i });
            }
        }

        [Test]
        public void Try_split_empty_group_test()
        {
            IGroup g = new Group();
            IEnumerable<IGroup> result = splitter.Split(g, 1);
            Assert.IsTrue(result.Count() == 0);
        }

        [Test]
        public void Try_split_custom_group_test()
        {
            int factor = 10;
            IEnumerable<IGroup> result = splitter.Split(group, factor).ToList();
            Assert.AreEqual(factor - 1, result.Count());
            foreach (IGroup g in result)
            {
                Assert.AreEqual(wordCount / factor, g.Words.Count);
            }
        }

        [Test]
        public void Try_split_custom_group_on_not_equal_test()
        {
            int factor = 11;
            IEnumerable<IGroup> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == factor - 1);
            foreach (IGroup g in result)
            {
                Assert.IsTrue(g.Words.Count == wordCount / factor || g.Words.Count == wordCount / factor - 1);
            }
        }

        [Test]
        public void Try_split_group_with_exteme_factor_test()
        {
            IEnumerable<IGroup> result = splitter.Split(group, 0);
            Assert.IsTrue(result.Count() == 0);

            result = splitter.Split(group, 1);
            Assert.IsTrue(result.Count() == 0);

            result = splitter.Split(group, result.Count());
            Assert.IsTrue(result.Count() == 0);
        }

    }
}
