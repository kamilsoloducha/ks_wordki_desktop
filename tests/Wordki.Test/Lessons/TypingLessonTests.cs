using NUnit.Framework;
using Oazachaosu.Core.Common;
using System.Collections.Generic;
using System.Linq;
using Wordki.Helpers.WordComparer;
using Wordki.Models;
using Wordki.Models.Lesson;
using WordkiModel;
using WordkiModel.Extensions;

namespace Wordki.Test.Lessons
{
    [TestFixture]
    public class TypingLessonTests
    {

        private Lesson lesson;
        private int wordCount = 10;

        [SetUp]
        public void Init()
        {
            lesson = new TypingLesson();
            lesson.WordComparer = GetWordComparer();
            lesson.LessonSettings = GetLessonSettings();
            lesson.InitLesson(GetWords());
        }

        [Test]
        public void Check_correct_word_test()
        {
            lesson.NextWord();
            IWord selectedWord = lesson.SelectedWord;
            lesson.Check(selectedWord.Language2);
            Assert.IsTrue(lesson.IsCorrect);
        }

        [Test]
        public void Check_wrong_word_test()
        {
            lesson.NextWord();
            IWord selectedWord = lesson.SelectedWord;
            lesson.Check("");
            Assert.IsFalse(lesson.IsCorrect);
        }

        [Test]
        public void Known_correct_word_test()
        {
            lesson.NextWord();
            IWord selectedWord = lesson.SelectedWord;
            lesson.Check(selectedWord.Language2);
            lesson.Known();
            Assert.AreEqual(4, selectedWord.Drawer, "selectedWord.Drawer is not expected");
            Assert.AreEqual(wordCount - 2, lesson.WordQueue.Count, "lesson.WordQueue.Count is not expexted");
        }

        [Test]
        public void Known_wrong_word_test()
        {
            lesson.NextWord();
            IWord selectedWord = lesson.SelectedWord;
            lesson.Check("");
            lesson.Known();
            Assert.AreEqual(4, selectedWord.Drawer, "selectedWord.Drawer is not expected");
            Assert.AreEqual(wordCount - 2, lesson.WordQueue.Count, "lesson.WordQueue.Count is not expexted");
        }

        [Test]
        public void Unknown_wrong_word_test()
        {
            lesson.NextWord();
            IWord selectedWord = lesson.SelectedWord;
            lesson.Check("");
            lesson.Unknown();
            Assert.AreEqual(0, selectedWord.Drawer, "selectedWord.Drawer is not expected");
            Assert.AreEqual(wordCount - 1, lesson.WordQueue.Count, "lesson.WordQueue.Count is not expexted");
        }

        [Test]
        public void Known_correct_word_check_result_test()
        {
            lesson.NextWord();
            IWord selectedWord = lesson.SelectedWord;
            lesson.Check(selectedWord.Language2);
            lesson.Known();
            IResult result = lesson.ResultList.Single(x => x.Group == selectedWord.Group);
            Assert.AreEqual(1, result.Correct);
        }

        [Test]
        public void Known_wrong_word_check_results_test()
        {
            lesson.NextWord();
            IWord selectedWord = lesson.SelectedWord;
            lesson.Check("");
            lesson.Known();
            IResult result = lesson.ResultList.Single(x => x.Group == selectedWord.Group);
            Assert.AreEqual(1, result.Accepted);
        }

        [Test]
        public void Unknown_wrong_word_check_results_test()
        {
            lesson.NextWord();
            IWord selectedWord = lesson.SelectedWord;
            lesson.Check("");
            lesson.Unknown();
            IResult result = lesson.ResultList.Single(x => x.Group == selectedWord.Group);
            Assert.AreEqual(1, result.Wrong);
        }

        [Test]
        public void Check_lessons_duration_if_all_correct_test()
        {
            lesson.NextWord();
            int counter = 0;
            do
            {
                counter++;
                IWord selectedWord = lesson.SelectedWord;
                lesson.Check(selectedWord.Language2);
                lesson.Known();
            } while (lesson.SelectedWord != null);
            Assert.AreEqual(wordCount, counter, "Lesson duration is not as exepected");
            IResult result = lesson.ResultList.First();
            Assert.AreEqual(wordCount, result.Correct, "Correct number in result is not as expected");
        }

        [Test]
        public void Check_lessons_duration_if_all_wrong_but_accepted_test()
        {
            lesson.NextWord();
            int counter = 0;
            do
            {
                counter++;
                IWord selectedWord = lesson.SelectedWord;
                lesson.Check("");
                lesson.Known();
            } while (lesson.SelectedWord != null);
            Assert.AreEqual(wordCount, counter, "Lesson duration is not as exepected");
            IResult result = lesson.ResultList.First();
            Assert.AreEqual(wordCount, result.Accepted, "Correct number in result is not as expected");
        }

        [Test]
        public void Check_lessons_duration_if_all_wrong_and_then_correct()
        {
            lesson.NextWord();
            int counter = 0;
            do
            {
                counter++;
                IWord selectedWord = lesson.SelectedWord;
                lesson.Check("");
                lesson.Unknown();
            } while (counter < wordCount);
            do
            {
                counter++;
                IWord selectedWord = lesson.SelectedWord;
                lesson.Check(selectedWord.Language2);
                lesson.Known();
            } while (lesson.SelectedWord != null);
            Assert.AreEqual(wordCount * 2, counter, "Lesson duration is not as exepected");
            IResult result = lesson.ResultList.First();
            Assert.AreEqual(wordCount, result.Wrong, "Correct number in result is not as expected");
            foreach (IWord word in lesson.BeginWordsList)
            {
                Assert.AreEqual(0, word.Drawer, "Drawer is not as expected");
            }
        }

        private IEnumerable<IWord> GetWords()
        {
            IGroup group = Utility.GetGroup();
            for (int i = 0; i < wordCount; i++)
            {
                IWord word = new Word()
                {
                    Language1 = $"Language{i}",
                    Language2 = $"Jezyk{i}",
                    Drawer = 3,
                };
                group.AddWord(word);
                yield return word;
            }
        }

        private IWordComparer GetWordComparer()
        {
            IWordComparer comparer = new WordComparer();
            comparer.Settings = new WordComparerSettings();
            comparer.Settings.WordSeparator = ',';
            comparer.Settings.NotCheckers.Add(new LetterCaseNotCheck());
            comparer.Settings.NotCheckers.Add(new SpaceNotCheck());
            comparer.Settings.NotCheckers.Add(new Utf8NotCheck());
            return comparer;
        }

        private ILessonSettings GetLessonSettings()
        {
            return new LessonSettings()
            {
                AllWords = true,
                TranslationDirection = TranslationDirection.FromFirst,
            };
        }
    }
}
