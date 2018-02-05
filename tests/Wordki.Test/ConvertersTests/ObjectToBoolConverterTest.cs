using NUnit.Framework;
using System.Globalization;
using Wordki.Helpers.Converters;

namespace Wordki.Test.ConvertersTests
{
    [TestFixture]
    public class ObjectToBoolConverterTest
    {

        ObjectToBoolConverter converter;

        [SetUp]
        public void SetUp()
        {
            converter = new ObjectToBoolConverter();
        }

        [Test]
        public void Object_to_true_test()
        {
            bool result = (bool) converter.Convert(new object(), typeof(bool), null, CultureInfo.CurrentUICulture);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Null_to_false_test()
        {
            bool result = (bool)converter.Convert(null, typeof(bool), null, CultureInfo.CurrentUICulture);
            Assert.AreEqual(false, result);
        }

    }
}
