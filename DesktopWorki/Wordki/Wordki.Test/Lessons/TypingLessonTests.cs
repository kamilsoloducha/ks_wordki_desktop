using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;
using Wordki.Models.Lesson;

namespace Wordki.Test.Lessons
{
    [TestClass]
    public class TypingLessonTests
    {

        private Lesson lesson;
        private IEnumerable<Word> words;
        private int wordCount = 10;

        [TestInitialize]
        public void Init()
        {
            lesson = new TypingLesson();
        }


        private IEnumerable<Word> GetWords()
        {
            for (int i = 0; i < wordCount; i++)
            {
                yield return new Word()
                {
                    Language1 = $"Language{i}",
                    Language2 = $"Jezyk{i}",
                    Drawer = 3,
                };
            }
        }
    }
}
