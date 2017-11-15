using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wordki.Helpers.WordComparer;

namespace Wordki.Test.Helpers
{
    [TestClass]
    public class WordComparerTest
    {
        IWordComparer comparer;


        [TestInitialize]
        public void Init()
        {
            comparer = new WordComparer();
            comparer.Settings = new WordComparerSettings();
        }

        [TestMethod]
        public void Compare_Same_Word_Without_Not_Checkers_Test()
        {
            string word1 = "test";
            string word2 = "test";
            Assert.IsTrue(comparer.IsEqual(word1, word2));
        }

        [TestMethod]
        public void Compare_Not_Same_Word_Without_Not_Checkers_Test()
        {
            string word1 = "test";
            string word2 = "test2";
            Assert.IsFalse(comparer.IsEqual(word1, word2));
        }

    }
}
