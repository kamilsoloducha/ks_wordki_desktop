using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models.Lesson;

namespace Wordki.Test.Lessons
{
    [TestFixture(1)]
    [TestFixture(10)]
    [Category("Lesson")]
    public class CheckInitialization
    {

        Lesson lesson;
        int groupCount = -1;

        public CheckInitialization(int groupCount)
        {
            this.groupCount = groupCount;
        }

        [SetUp]
        public void SetUp()
        {
            lesson = new TypingLesson();
            lesson.WordComparer = LessonUtil.GetWordComparer();
            lesson.LessonSettings = LessonUtil.GetLessonSettings();
            lesson.InitLesson(Utility.GetGroups(groupCount).SelectMany(x => x.Words));
        }

        [Test]
        public void Check_begin_words_count_test()
        {
            Assert.AreEqual(groupCount * Utility.WordCount, lesson.BeginWordsList.Count);
        }

        [Test]
        public void Check_words_queue_count_test()
        {
            Assert.AreEqual(groupCount * Utility.WordCount, lesson.WordQueue.Count);
        }

        [Test]
        public void Check_results_count_test()
        {
            Assert.AreEqual(groupCount, lesson.ResultList.Count);
        }
    }
}
