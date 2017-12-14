using NUnit.Framework;
using Repository.Models;
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
    public class CheckUnknownWrongWord
    {

        Lesson lesson;
        IWord selectedWord;
        int groupCount = -1;

        public CheckUnknownWrongWord(int groupCount)
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
            selectedWord = lesson.SelectedWord;
            lesson.Check(" ");
            lesson.Unknown();
        }

        [Test]
        public void Check_result_correct_if_correct_word()
        {
            IResult result = lesson.ResultList.First(x => x.Group == selectedWord.Group);
            Assert.AreEqual(0, result.Correct);
        }

        [Test]
        public void Check_result_accepted_if_correct_word()
        {
            IResult result = lesson.ResultList.First(x => x.Group == selectedWord.Group);
            Assert.AreEqual(0, result.Accepted);
        }

        [Test]
        public void Check_result_wrong_if_correct_word()
        {
            IResult result = lesson.ResultList.First(x => x.Group == selectedWord.Group);
            Assert.AreEqual(1, result.Wrong);
        }

        [Test]
        public void Check_drawer_if_correct_word()
        {
            Assert.AreEqual(0, selectedWord.Drawer);
        }

        [Test]
        public void Check_remaining_word_count()
        {
            Assert.AreEqual(groupCount * Utility.WordCount - 1, lesson.WordQueue.Count);
        }
    }
}
