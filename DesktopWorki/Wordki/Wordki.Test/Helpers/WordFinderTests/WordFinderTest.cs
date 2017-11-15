using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Test.Helpers.WordFinderTests
{
    [TestClass]
    public class WordFinderTest
    {

        [TestMethod]
        public void test()
        {
            IEnumerable<int> result = t();
            Assert.IsTrue(result.Count() == 200);
        }

        public IEnumerable<int> t()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(i);
            }

            foreach (var item in list)
            {

                yield return item;
                yield return item;
            }
        }

    }
}
