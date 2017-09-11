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
        GroupSplitterBase splitter;
        Group group;
        int wordCount = 100;
        int factor = 50;
        IEnumerable<Group> result;

        [TestInitialize]
        public void Init()
        {
            splitter = new GroupSlitPercentage();
            group = new Group();
            for (int i = 0; i < wordCount; i++)
            {
                group.WordsList.Add(new Word());
            }

            result = splitter.Split(group, factor);
        }

        [TestMethod]
        public void Try_split_custom_group_test()
        {
            Assert.IsTrue(result.Count() == 1);
            Group x = result.FirstOrDefault();
            Assert.IsTrue(x != null);
            Assert.IsTrue(x.WordsList.Count == 50);
        }

    }
}
