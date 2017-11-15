using NUnit.Framework;
using Repository.Helper;
using Wordki.Models;

namespace Wordki.Test.RepositoryTest
{
    [TestFixture]
    public class DirectionSwaperTests
    {

        private IDirectionSwaper swaper;
        private Result result;

        [SetUp]
        public void SetUp()
        {
            swaper = new DirectionSwaper();
            result = new Result()
            {
                TranslationDirection = Repository.Models.Enums.TranslationDirection.FromFirst,
            };
        }

        [Test]
        public void Swap_single_result_test()
        {
            swaper.Swap(result);
            Assert.AreEqual(Repository.Models.Enums.TranslationDirection.FromSecond, result.TranslationDirection);
        }

    }
}
