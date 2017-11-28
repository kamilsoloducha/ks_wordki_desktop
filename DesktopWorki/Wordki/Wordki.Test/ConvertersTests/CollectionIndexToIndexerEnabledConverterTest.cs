using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.ViewModels.Dialogs;

namespace Wordki.Test.ConvertersTests
{
    [TestFixture]
    public class CollectionIndexToIndexerEnabledConverterTest
    {

        Utility util = new Utility();
        CollectionIndexToIndexerEnabledConverter converter;
        List<object> list;

        [SetUp]
        public void SetUp()
        {
            converter = new CollectionIndexToIndexerEnabledConverter();
            list = new List<object> { new object(), new object(), new object(), new object(), new object(), new object(), new object() };
        }

        [Test]
        public void Is_true_if_search_first_element_test()
        {
            converter.Index = CheckingElement.First;
            bool result = (bool)converter.Convert(list[0], typeof(bool), list, CultureInfo.CurrentUICulture);
            Assert.IsTrue(result);
        }

        [Test]
        public void Is_false_if_search_a_element_test()
        {
            converter.Index = CheckingElement.First;
            bool result = (bool)converter.Convert(list[2], typeof(bool), list, CultureInfo.CurrentUICulture);
            Assert.IsFalse(result);
        }

        [Test]
        public void Is_true_if_search_last_element_test()
        {
            converter.Index = CheckingElement.Last;
            bool result = (bool)converter.Convert(list.Last(), typeof(bool), list, CultureInfo.CurrentUICulture);
            Assert.IsTrue(result);
        }

        [Test]
        public void Is_false_if_search_a_element_from_last_test()
        {
            converter.Index = CheckingElement.Last;
            bool result = (bool)converter.Convert(list[2], typeof(bool), list, CultureInfo.CurrentUICulture);
            Assert.IsFalse(result);
        }

    }
}
