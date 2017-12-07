using Repository.Models;
using System;
using System.Collections.Generic;

namespace Wordki.Models.Lesson
{
    [Serializable]
    public class RandomLesson : TypingLesson
    {
        public RandomLesson() : base() { }

        protected override void CreateWordList(IEnumerable<IWord> words)
        {
            foreach (Word word in words)
            {
                BeginWordsList.Add((IWord)word.Clone());
            }
            foreach (IWord word in BeginWordsList)
            {
                WordList.Enqueue(word);
            }
        }

        protected override void CreateResultList()
        {
            ResultList = new List<IResult>();
        }
    }
}
