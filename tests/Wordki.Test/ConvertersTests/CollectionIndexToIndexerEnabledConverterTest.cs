using NUnit.Framework;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Wordki.Helpers.Converters;

namespace Wordki.Test.ConvertersTests
{
    [TestFixture]
    public class CollectionIndexToIndexerEnabledConverterTest
    {

        ItemIndexToBoolConverter converter;
        List<object> list;

        [SetUp]
        public void SetUp()
        {
            converter = new ItemIndexToBoolConverter();
            list = new List<object> { "1", "2", "3", "4", "5", "6", "7" };
        }

        [Test]
        public void Is_true_if_search_first_element_test()
        {
            converter.Index = CheckingElement.First;
            bool result = (bool)converter.Convert(new object[] { list[0], list }, typeof(bool), null, CultureInfo.CurrentUICulture);
            Assert.IsFalse(result);
        }

        [Test]
        public void Is_false_if_search_a_element_test()
        {
            converter.Index = CheckingElement.First;
            bool result = (bool)converter.Convert(new object[] { list[2], list }, typeof(bool), null, CultureInfo.CurrentUICulture);
            Assert.IsTrue(result);
        }

        [Test]
        public void Is_true_if_search_last_element_test()
        {
            converter.Index = CheckingElement.Last;
            bool result = (bool)converter.Convert(new object[] { list.Last(), list }, typeof(bool), null, CultureInfo.CurrentUICulture);
            Assert.IsFalse(result);
        }

        [Test]
        public void Is_false_if_search_a_element_from_last_test()
        {
            converter.Index = CheckingElement.Last;
            bool result = (bool)converter.Convert(new object[] { list[2], list }, typeof(bool), null, CultureInfo.CurrentUICulture);
            Assert.IsTrue(result);
        }

    }
}
