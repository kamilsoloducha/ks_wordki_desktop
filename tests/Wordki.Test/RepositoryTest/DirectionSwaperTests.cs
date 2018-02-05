using NUnit.Framework;
using Oazachaosu.Core.Common;
using Wordki.Models;

using WordkiModel;

namespace Wordki.Test.RepositoryTest
{
    [TestFixture]
    public class DirectionSwaperTests
    {

        private Result result;

        [SetUp]
        public void SetUp()
        {
            result = new Result()
            {
                TranslationDirection = TranslationDirection.FromFirst,
            };
        }

        [Test]
        public void Swap_single_result_test()
        {
            result.ChangeDirection();
            Assert.AreEqual(TranslationDirection.FromSecond, result.TranslationDirection);
        }

    }
}
