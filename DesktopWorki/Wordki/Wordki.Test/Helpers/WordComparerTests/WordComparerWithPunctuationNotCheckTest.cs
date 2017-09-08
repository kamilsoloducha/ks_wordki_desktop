using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models.Lesson.WordComparer;

namespace Wordki.Test.Helpers.WordComparerTests
{
    [TestClass]
    public class WordComparerWithPunctuationNotCheckTest
    {


        INotCheck notCheck;
        IWordComparer comparer;

        [TestInitialize]
        public void Init()
        {
            notCheck = new PunctuationNotCheck();
            comparer = new WordComparer();
        }

        [TestMethod]
        public void Compare_Not_Same_Word_Without_Utf8_Not_Check_Test()
        {
            string word1 = "test-";
            string word2 = "test";
            Assert.IsFalse(comparer.Compare(word1, word2));
        }

        [TestMethod]
        public void Compare_Same_Word_With_utf8_Not_Check_Test()
        {
            string word1 = "test";
            string word2 = "test";
            comparer.NotCheckers.Add(notCheck);
            Assert.IsTrue(comparer.Compare(word1, word2));
        }

        [TestMethod]
        public void Compare_Not_Same_Word_With_Letter_Not_Check_Test()
        {
            string word1 = "t$e2st";
            string word2 = "t!est-";
            comparer.NotCheckers.Add(notCheck);
            Assert.IsTrue(comparer.Compare(word1, word2));
        }

        [TestMethod]
        public void Compare_Wrong_Word_With_Letter_Not_Check_Test()
        {
            string word1 = "Test2";
            string word2 = "test";
            comparer.NotCheckers.Add(notCheck);
            Assert.IsFalse(comparer.Compare(word1, word2));
        }

    }
}
