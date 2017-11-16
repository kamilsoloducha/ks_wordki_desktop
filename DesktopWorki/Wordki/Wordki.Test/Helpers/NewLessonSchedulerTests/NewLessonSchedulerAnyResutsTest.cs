using NUnit.Framework;
using Repository.Models;
using Wordki.Models;
using Wordki.Models.LessonScheduler;

namespace Wordki.Test.Helpers.NewLessonSchedulerTests
{
    [TestFixture]
    public class NewLessonSchedulerAnyResutsTest
    {

        ILessonScheduleInitializer initializer;
        ILessonScheduler lessonScheduler;
        int[] days = new int[] { 1, 2, 3, 4, 5 };

        [SetUp]
        public void SetUp()
        {
            initializer = new LessonSchedulerInitializer2(days);
            lessonScheduler = new NewLessonScheduler()
            {
                Initializer = initializer,
            };
        }

        [Test]
        public void Return_zero_if_group_has_no_result_test()
        {
            int daysToRepetition = lessonScheduler.GetTimeToLearn(new Group());
            Assert.AreEqual(0, daysToRepetition);
        }

        [Test]
        public void Retrun_specific_number_if_group_has_specific_count_of_result_test()
        {
            IGroup group = new Group();
            for (int i = 0; i < days.Length + 4; i++)
            {
                group.AddResult(new Result());
                int daysToRepetition = lessonScheduler.GetTimeToLearn(group);
                int expected = i >= days.Length ? days[days.Length - 1] : days[i];
                Assert.AreEqual(expected, daysToRepetition, $"Wrong value for {i} iteration");
            }
        }
    }
}
