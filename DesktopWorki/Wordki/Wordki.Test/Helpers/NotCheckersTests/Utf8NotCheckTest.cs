using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Helpers.WordComparer;

namespace Wordki.Test.Helpers
{
    [TestClass]
    public class Utf8NotCheckTest
    {

        [TestMethod]
        public void AsciiTextConvertTest()
        {
            string textBefore = "qwertyuiopasdfghjklzxcvbnm1234567890`~!@#$%^&*()-=_+[]{}\\|;':\",./<>?";
            string textAfter = "qwertyuiopasdfghjklzxcvbnm1234567890`~!@#$%^&*()-=_+[]{}\\|;':\",./<>?";
            Utf8NotCheck converter = new Utf8NotCheck();
            Assert.IsTrue(textAfter.Equals(converter.Convert(textBefore)));
        }

        [TestMethod]
        public void PolishTextConvertTest()
        {
            string textBefore = "ąśżźćńęłóĄŚŻŹĆŁÓŃĘ";
            string textAfter = "aszzcneloASZZCLONE";
            Utf8NotCheck converter = new Utf8NotCheck();
            string convertedText = converter.Convert(textBefore).ToString();
            Assert.IsTrue(textAfter.Equals(convertedText));
        }


    }
}
