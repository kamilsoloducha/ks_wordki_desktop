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
    public class LetterCaseNotCheckTest
    {

        [TestMethod]
        public void OnlyLowerCasesTextTest()
        {
            string textBefore = "sometext.asdf";
            string textAfter = "sometext.asdf";
            INotCheck notChecker = new LetterCaseNotCheck();
            string textConverted = notChecker.Convert(textBefore);
            Assert.IsTrue(textAfter.Equals(textConverted));
        }

        [TestMethod]
        public void SomeUpperCasesTextTest()
        {
            string textBefore = "sOmeTexT.aSdf";
            string textAfter = "sometext.asdf";
            INotCheck notChecker = new LetterCaseNotCheck();
            string textConverted = notChecker.Convert(textBefore);
            Assert.IsTrue(textAfter.Equals(textConverted));
        }

    }
}
