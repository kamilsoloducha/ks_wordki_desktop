using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wordki.Models.Lesson.WordComparer;

namespace Wordki.Test.Helpers.WordComparerTests
{
    [TestClass]
    public class WordComparerWithLetterCaseNotCheckTest
    {

        IWordComparer comparer;
        INotCheck letterCaseNotCheck;


        [TestInitialize]
        public void Init()
        {
            comparer = new WordComparer();
            letterCaseNotCheck = new LetterCaseNotCheck();
        }

        [TestMethod]
        public void Compare_Same_Word_With_Letter_Not_Check_Test()
        {
            string word1 = "test";
            string word2 = "test";
            comparer.NotCheckers.Add(letterCaseNotCheck);
            Assert.IsTrue(comparer.Compare(word1, word2));
        }

        [TestMethod]
        public void Compare_Not_Same_Word_With_Letter_Not_Check_Test()
        {
            string word1 = "teSt";
            string word2 = "Test";
            comparer.NotCheckers.Add(letterCaseNotCheck);
            Assert.IsTrue(comparer.Compare(word1, word2));
        }

        [TestMethod]
        public void Compare_Wrong_Word_With_Letter_Not_Check_Test()
        {
            string word1 = "test2";
            string word2 = "Test";
            comparer.NotCheckers.Add(letterCaseNotCheck);
            Assert.IsFalse(comparer.Compare(word1, word2));
        }

    }
}
