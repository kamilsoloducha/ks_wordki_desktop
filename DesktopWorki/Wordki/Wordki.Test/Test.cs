using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Wordki.Test
{
    [TestFixture]
    public class Test
    {

        [Test]
        public void test1()
        {
            Console.WriteLine("Start");
            int result = GetAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("After Call");
            Console.WriteLine($"Result: {result}");
        }

        public Task<int> GetAsync()
        {
            return Task.Run<int>(() =>
            {
                Console.WriteLine("Before Delay");
                Task.Delay(1000);
                Console.WriteLine("After Delay");
                return 5;
            });
        }

    }
}
