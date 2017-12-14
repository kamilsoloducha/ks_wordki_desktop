using NUnit.Framework;
using Repository.Models;
using System.Linq;
using Wordki.Models.Lesson;

namespace Wordki.Test.Lessons
{
    [TestFixture(1)]
    [TestFixture(10)]
    [Category("Lesson")]
    public class CheckCheckingWord
    {
        Lesson lesson;
        int groupCount = -1;

        public CheckCheckingWord(int groupCount)
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
            lesson.NextWord();
        }

        [Test]
        public void Check_is_correct()
        {
            IWord word = lesson.SelectedWord;
            lesson.Check(word.Language2);

            Assert.IsTrue(lesson.IsCorrect);
        }

        [Test]
        public void Check_is_correct_if_wrong_word()
        {
            lesson.Check(" ");
            Assert.IsFalse(lesson.IsCorrect);
        }

        [Test]
        public void Check_word_queue_count()
        {
            Assert.AreEqual(groupCount * Utility.WordCount - 1, lesson.WordQueue.Count);
        }
    }
}
