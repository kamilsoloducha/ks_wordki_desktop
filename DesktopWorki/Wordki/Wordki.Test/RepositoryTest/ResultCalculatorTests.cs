using NUnit.Framework;
using Repository.Models;
using System;
using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Test.RepositoryTest
{
    [TestFixture]
    public class ResultCalculatorTests
    {
        IResultOrganizer calculator;
        IList<IGroup> groups;
        DateTime date = new DateTime(1990, 9, 24, 12, 0, 0);
        [SetUp]
        public void SetUp()
        {
            groups = new List<IGroup>();
            calculator = new ResultOrganizer()
            {
                Groups = groups,
            };
        }

        [Test]
        public void Get_time_with_no_groups_test()
        {
            Assert.AreEqual(0, calculator.GetLessonTime(date.AddDays(-1), date));
        }

        [Test]
        public void Get_time_with_one_group_no_results_test()
        {
            groups.Add(new Group());
            Assert.AreEqual(0, calculator.GetLessonTime(date.AddDays(-1), date));
        }

        [Test]
        public void Get_time_with_result_between_scope_test()
        {
            IGroup group = new Group();
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddHours(-10) });
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddHours(-12) });
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddHours(-5) });
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddHours(-2) });
            groups.Add(group);
            Assert.AreEqual(4, calculator.GetLessonTime(date.AddDays(-1), date));
        }

        [Test]
        public void Get_time_with_result_outside_scope_test()
        {
            IGroup group = new Group();
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddDays(-10) });
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddDays(-12) });
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddDays(-5) });
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddDays(-2) });
            groups.Add(group);
            Assert.AreEqual(0, calculator.GetLessonTime(date.AddDays(-1), date));
        }

        [Test]
        public void Get_time_with_result_on_all_scope_test()
        {
            IGroup group = new Group();
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddDays(-10) });
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddHours(-12) });
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddHours(-5) });
            group.AddResult(new Result() { TimeCount = 1, DateTime = date.AddDays(-2) });
            groups.Add(group);
            Assert.AreEqual(2, calculator.GetLessonTime(date.AddDays(-1), date));
        }
    }
}
