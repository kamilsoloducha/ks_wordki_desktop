using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;
using Wordki.Helpers.GroupSplitter;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Test.Helpers.GroupSplitterTests
{
    [TestFixture]
    public class GroupSplitterPercentageTest
    {
        private GroupSplitterBase splitter;
        private IGroup group;
        private int wordCount = 100;

        [SetUp]
        public void Init()
        {
            splitter = new GroupSlitPercentage();
            group = new Group();
            for (int i = 0; i < wordCount; i++)
            {
                group.Words.Add(new Word() { Group = group, Id = i });
            }
        }

        [Test]
        public void Try_split_empty_group_test()
        {
            int factor = 1;
            IGroup g = new Group();
            IEnumerable<IGroup> result = splitter.Split(g, factor);
            Assert.IsTrue(result.Count() == 0);
        }


        [Test]
        public void Try_split_custom_group_on_half_test()
        {
            int factor = 50;
            IEnumerable<IGroup> result = splitter.Split(group, factor).ToList();
            Assert.AreEqual(1, result.Count());
            IGroup x = result.FirstOrDefault();
            Assert.IsNotNull(x);
            Assert.AreEqual(wordCount * factor / 100, x.Words.Count);
        }

        [Test]
        public void Try_split_group_with_100_factor_test()
        {
            int factor = 100;
            IEnumerable<IGroup> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == 0);
        }

        [Test]
        public void Try_split_group_with_0_factor_test()
        {
            int factor = 0;
            IEnumerable<IGroup> result = splitter.Split(group, factor);
            Assert.IsTrue(result.Count() == 0);
        }

        [Test]
        public void Try_split_group_with_random_factor_test()
        {
            int factor = 23;
            IEnumerable<IGroup> result = splitter.Split(group, factor).ToList();
            Assert.IsTrue(result.Count() == 1);
            IGroup x = result.FirstOrDefault();
            Assert.IsTrue(x != null);
            Assert.IsTrue(x.Words.Count == 100 - factor);
        }

        [Test]
        public void Try_split_group_with_odd_words_count_test()
        {
            group.Words.RemoveAt(0);
            int factor = 50;
            IEnumerable<IGroup> result = splitter.Split(group, factor).ToList();
            Assert.IsTrue(result.Count() == 1);
            IGroup x = result.FirstOrDefault();
            Assert.IsTrue(x != null);
            Assert.IsTrue(x.Words.Count == 50);
        }

    }
}
