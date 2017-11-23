using NUnit.Framework;
using Repository.Models;
using Repository.Models.Enums;
using Wordki.Helpers.ResultConnector;

namespace Wordki.Test.Helpers.ResultConnecterTests
{
    [TestFixture]
    public class ConnectTwoGroupsTests
    {

        IResultConnector connector;
        Utility util = new Utility()
        {
            ResultCount = 10,
            WordCount = 0,
        };

        [SetUp]
        public void Setup()
        {
            connector = new ResultConnector();
        }

        [Test]
        public void Connect_two_groups_one_without_results_test()
        {
            IGroup dest = util.GetGroup();
            util.ResultCount = 0;
            IGroup src = util.GetGroup();
            connector.Connect(dest, src);
            Assert.AreEqual(0, dest.Results.Count);
        }

        [Test]
        public void Connect_two_groups_with_one_result_test()
        {
            util.ResultCount = 1;
            IGroup dest = util.GetGroup();
            IGroup src = util.GetGroup();
            connector.Connect(dest, src);
            Assert.AreEqual(1, dest.Results.Count);
        }

        [Test]
        public void Connect_two_groups_with_another_type_lesson_test()
        {
            util.ResultCount = 0;
            IGroup dest = util.GetGroup();
            IGroup src = util.GetGroup();
            dest.AddResult(util.GetResult(lessonType: LessonType.FiszkiLesson));
            src.AddResult(util.GetResult(lessonType: LessonType.TypingLesson));
            connector.Connect(dest, src);
            Assert.AreEqual(0, dest.Results.Count);
        }

        [Test]
        public void Connect_two_groups_with_swap_lesson_type_test()
        {
            util.ResultCount = 0;
            IGroup dest = util.GetGroup();
            IGroup src = util.GetGroup();
            dest.AddResult(util.GetResult(lessonType: LessonType.FiszkiLesson));
            dest.AddResult(util.GetResult(lessonType: LessonType.TypingLesson));
            src.AddResult(util.GetResult(lessonType: LessonType.TypingLesson));
            src.AddResult(util.GetResult(lessonType: LessonType.FiszkiLesson));
            connector.Connect(dest, src);
            Assert.AreEqual(2, dest.Results.Count);
        }

        [Test]
        public void Connect_two_groups_with_diffrent_number_of_results_test()
        {
            util.ResultCount = 2;
            IGroup dest = util.GetGroup();
            util.ResultCount = 4;
            IGroup src = util.GetGroup();
            connector.Connect(dest, src);
            Assert.AreEqual(2, dest.Results.Count);
        }

    }
}
