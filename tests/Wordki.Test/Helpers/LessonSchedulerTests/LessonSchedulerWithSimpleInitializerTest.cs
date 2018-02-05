using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using Wordki.Models;
using Wordki.Models.LessonScheduler;
using WordkiModel;
using WordkiModel.Extensions;

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
            IGroup group = new Group();
            group.AddResult(result);
            int r = scheduler.GetColor(group);
            Assert.AreEqual(0, r);

            result = new Result()
            {
                DateTime = DateTime.Now.AddDays(-7),
            };
            group.AddResult(result);
            r = scheduler.GetColor(group);
            Assert.AreEqual(3, r);

            result = new Result()
            {
                DateTime = DateTime.Now.AddDays(-9),
            };
            group.AddResult(result);
            r = scheduler.GetColor(group);
            Assert.AreEqual(4, r);
        }
    }
}
