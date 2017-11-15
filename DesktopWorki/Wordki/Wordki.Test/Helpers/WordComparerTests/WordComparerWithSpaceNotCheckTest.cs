using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wordki.Helpers.WordComparer;

namespace Wordki.Test.Helpers.WordComparerTests
{

    [TestClass]
    public class WordComparerWithSpaceNotCheckTest
    {

        INotCheck notCheck;
        IWordComparer comparer;

        [TestInitialize]
        public void Init()
        {
            notCheck = new SpaceNotCheck();
            comparer = new WordComparer();
            comparer.Settings = new WordComparerSettings();
        }

        [TestMethod]
        public void Compare_Not_Same_Word_Without_Utf8_Not_Check_Test()
        {
            string word1 = "test ";
            string word2 = "test";
            Assert.IsFalse(comparer.IsEqual(word1, word2));
        }

        [TestMethod]
        public void Compare_Same_Word_With_utf8_Not_Check_Test()
        {
            string word1 = "test";
            string word2 = "test";
            comparer.Settings.NotCheckers.Add(notCheck);
            Assert.IsTrue(comparer.IsEqual(word1, word2));
        }

        [TestMethod]
        public void Compare_Not_Same_Word_With_Letter_Not_Check_Test()
        {
            string word1 = "test";
            string word2 = "test ";
            comparer.Settings.NotCheckers.Add(notCheck);
            Assert.IsTrue(comparer.IsEqual(word1, word2));
        }

        [TestMethod]
        public void Compare_Wrong_Word_With_Letter_Not_Check_Test()
        {
            string word1 = "test2";
            string word2 = "test";
            comparer.Settings.NotCheckers.Add(notCheck);
            Assert.IsFalse(comparer.IsEqual(word1, word2));
        }

    }
}
