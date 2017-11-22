using NUnit.Framework;
using Repository.Models;
using Wordki.Helpers.ResultConnector;
using Wordki.Models;

namespace Wordki.Test.Helpers.ResultConnecterTests
{
    [TestFixture]
    public class ConnectTwoResultTests
    {

        IResultConnector connector;
        IResult dest;
        IResult src;

        [SetUp]
        public void Setup()
        {
            connector = new ResultConnector();
            dest = new Result
            {
                Accepted = 10,
                Correct = 10,
                Wrong = 10,
                Invisibilities = 10,
            };
            src = new Result
            {
                Accepted = 2,
                Correct = 2,
                Wrong = 2,
                Invisibilities = 2,
            };
        }

        [Test]
        public void Connect_two_result_check_accepted_test()
        {
            connector.Connect(dest, src);
            Assert.AreEqual(12, dest.Accepted);
        }
        [Test]
        public void Connect_two_result_check_correct_test()
        {
            connector.Connect(dest, src);
            Assert.AreEqual(12, dest.Correct);
        }
        [Test]
        public void Connect_two_result_check_wrong_test()
        {
            connector.Connect(dest, src);
            Assert.AreEqual(12, dest.Wrong);
        }
        [Test]
        public void Connect_two_result_check_invisibilites_test()
        {
            connector.Connect(dest, src);
            Assert.AreEqual(12, dest.Invisibilities);
        }
    }
}
