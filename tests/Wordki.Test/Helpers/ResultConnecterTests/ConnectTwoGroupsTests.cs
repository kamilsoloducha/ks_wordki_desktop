using NUnit.Framework;
using Oazachaosu.Core.Common;
using Wordki.Helpers.ResultConnector;
using WordkiModel;

namespace Wordki.Test.Helpers.ResultConnecterTests
{
    [TestFixture]
    public class ConnectTwoGroupsTests
    {

        IResultConnector connector;
        [SetUp]
        public void Setup()
        {
            connector = new ResultConnector();
        }

        [Test]
        public void Connect_two_groups_one_without_results_test()
        {
            IGroup dest = Utility.GetGroup();
            Utility.ResultCount = 0;
            IGroup src = Utility.GetGroup();
            connector.Connect(dest, src);
            Assert.AreEqual(0, dest.Results.Count);
        }

        [Test]
        public void Connect_two_groups_with_one_result_test()
        {
            Utility.ResultCount = 1;
            IGroup dest = Utility.GetGroup();
            IGroup src = Utility.GetGroup();
            connector.Connect(dest, src);
            Assert.AreEqual(1, dest.Results.Count);
        }

        [Test]
        public void Connect_two_groups_with_another_type_lesson_test()
        {
            Utility.ResultCount = 0;
            IGroup dest = Utility.GetGroup();
            IGroup src = Utility.GetGroup();
            dest.AddResult(Utility.GetResult(lessonType: LessonType.FiszkiLesson));
            src.AddResult(Utility.GetResult(lessonType: LessonType.TypingLesson));
            connector.Connect(dest, src);
            Assert.AreEqual(0, dest.Results.Count);
        }

        [Test]
        public void Connect_two_groups_with_swap_lesson_type_test()
        {
            Utility.ResultCount = 0;
            IGroup dest = Utility.GetGroup();
            IGroup src = Utility.GetGroup();
            dest.AddResult(Utility.GetResult(lessonType: LessonType.FiszkiLesson));
            dest.AddResult(Utility.GetResult(lessonType: LessonType.TypingLesson));
            src.AddResult(Utility.GetResult(lessonType: LessonType.TypingLesson));
            src.AddResult(Utility.GetResult(lessonType: LessonType.FiszkiLesson));
            connector.Connect(dest, src);
            Assert.AreEqual(2, dest.Results.Count);
        }

        [Test]
        public void Connect_two_groups_with_diffrent_number_of_results_test()
        {
            Utility.ResultCount = 2;
            IGroup dest = Utility.GetGroup();
            Utility.ResultCount = 4;
            IGroup src = Utility.GetGroup();
            connector.Connect(dest, src);
            Assert.AreEqual(2, dest.Results.Count);
        }

    }
}
