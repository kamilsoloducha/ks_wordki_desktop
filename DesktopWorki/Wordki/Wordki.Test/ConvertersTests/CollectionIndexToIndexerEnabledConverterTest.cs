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

        [Test]
        public void Test()
        {
            CollectionIndexToIndexerEnabledConverter converter = new CollectionIndexToIndexerEnabledConverter();
            var list = util.GetGroup().Results;
            bool result = (bool)converter.Convert(list[0], typeof(bool), list, CultureInfo.CurrentUICulture);
            Assert.IsTrue(result);
        }

    }
}
