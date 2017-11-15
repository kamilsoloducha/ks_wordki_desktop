using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Wordki.Models;
using Wordki.Models.LessonScheduler;
using Wordki.Models.LessonScheduler.LessonScheduleInitializer;

namespace Wordki.Test.Helpers.LessonSchedulerTests
{
    [TestClass]
    public class LessonSchedulerWithSimpleInitializerTest
    {

        ILessonScheduler scheduler;
        ILessonScheduleInitializer scheduleInitializer;

        [TestInitialize]
        public void Init()
        {
            scheduleInitializer = new SimpleLessonScheduleInitializer();
            scheduler = new LessonScheduler(scheduleInitializer);
        }

        [TestMethod]
        public void Get_color_with_null_result_test()
        {
            int result = scheduler.GetColor(null);
            Assert.IsTrue(4 == result);
        }

        [TestMethod]
        public void Get_color_with_result_before_now_test()
        {
            Result result = new Result()
            {
                DateTime = DateTime.Now.AddDays(-1),
            };
            int r = scheduler.GetColor(result);
            Assert.AreEqual(0, r);

            result = new Result()
            {
                DateTime = DateTime.Now.AddDays(-7),
            };
            r = scheduler.GetColor(result);
            Assert.AreEqual(3, r);

            result = new Result()
            {
                DateTime = DateTime.Now.AddDays(-9),
            };
            r = scheduler.GetColor(result);
            Assert.AreEqual(4, r);
        }
    }
}
