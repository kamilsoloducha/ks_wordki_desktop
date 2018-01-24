using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Test.RepositoryTest
{
    [TestFixture]
    public class WordCalculatorTests
    {
        IWordCalculator calculator;
        IList<IGroup> groups;

        [SetUp]
        public void SetUp()
        {
            groups = new List<IGroup>();
            calculator = new WordCalculator()
            {
                Groups = groups,
            };
        }

        [Test]
        public void Get_drawers_with_no_groups_test()
        {
            foreach (int item in calculator.GetDrawerCount())
                Assert.AreEqual(0, item);
        }

        [Test]
        public void Get_drawers_with_group_without_words_test()
        {
            groups.Add(new Group());
            foreach (int item in calculator.GetDrawerCount())
                Assert.AreEqual(0, item);
        }

        [Test]
        public void Get_drawers_with_words_test()
        {
            IGroup group = new Group();
            group.AddWord(new Word() { Drawer = 0 });
            group.AddWord(new Word() { Drawer = 1 });
            group.AddWord(new Word() { Drawer = 2 });
            group.AddWord(new Word() { Drawer = 3 });
            group.AddWord(new Word() { Drawer = 4 });
            groups.Add(group);
            foreach (int item in calculator.GetDrawerCount())
                Assert.AreEqual(1, item);
        }

        [Test]
        public void Get_drawers_with_word_only_one_drawer_test()
        {
            IGroup group = new Group();
            group.AddWord(new Word() { Drawer = 1 });
            group.AddWord(new Word() { Drawer = 1 });
            group.AddWord(new Word() { Drawer = 1 });
            group.AddWord(new Word() { Drawer = 1 });
            group.AddWord(new Word() { Drawer = 1 });
            groups.Add(group);
            Assert.AreEqual(5, calculator.GetDrawerCount().ToArray()[1]);
        }

    }
}
