using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models.Lesson.WordComparer;

namespace Wordki.Test.Helpers
{
    [TestClass]
    public class WordComparerTest
    {
        IWordComparer comparer;

        [TestInitialize]
        public void Init()
        {
            comparer = new WordComparer2();
        }

        [TestMethod]
        public void Compare_Same_Word_Without_Not_Checkers_Test()
        {

        }

        [TestMethod]
        public void test()
        {

        }

    }
}
