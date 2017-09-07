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
    public class SpaceNotCheckTest
    {
        [TestMethod]
        public void AnySpaceTextTest()
        {
            string textBefore = "someText.asdf";
            string textAfter = "someText.asdf";
            INotCheck notChecker = new SpaceNotCheck();
            string textConverted = notChecker.Convert(new StringBuilder(textBefore)).ToString();
            Assert.IsTrue(textAfter.Equals(textConverted));
        }

        [TestMethod]
        public void TextWithSomeSpacesTest()
        {
            string textBefore = "s om eT ext. as df ";
            string textAfter = "someText.asdf";
            INotCheck notChecker = new SpaceNotCheck();
            string textConverted = notChecker.Convert(new StringBuilder(textBefore)).ToString();
            Assert.IsTrue(textAfter.Equals(textConverted));
        }
    }
}
