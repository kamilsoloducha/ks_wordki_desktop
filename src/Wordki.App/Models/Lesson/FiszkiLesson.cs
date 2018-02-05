using WordkiModel;

using System;
using System.Collections.Generic;
using System.Linq;
using Util.Collections;
using Oazachaosu.Core.Common;

namespace Wordki.Models.Lesson
{
    public class FiszkiLesson : Lesson
    {
        public FiszkiLesson() : base() { }

        public override void Check(string translation)
        {
            IsChecked = true;
        }

        public override void Known()
        {
            if (Counter++ <= BeginWordsList.Count)
            {
                IResult lResult = ResultList.FirstOrDefault(x => x.Group.Id == SelectedWord.Group.Id);
                SelectedWord.Drawer++;
                if (lResult != null)
                {
                    lResult.Correct++;
                }
            }
            NextWord();
        }

        protected override void CreateResultList()
        {
            ResultList = new List<IResult>();
            foreach (Word word in BeginWordsList)
            {
                if (ResultList.Count(x => x.Group == word.Group) != 0) continue;
                ResultList.Add(new Result(-1,
                  word.Group,
                  0,
                  0,
                  0,
                  (short)word.Group.Words.Count(x => !x.IsVisible),
                  0,
                  LessonSettings.TranslationDirection,
                  (LessonType)Enum.Parse(typeof(LessonType), GetType().Name),
                  DateTime.Now,
                  int.MaxValue));
            }
        }

        protected override void CreateWordList(IEnumerable<IWord> words)
        {
            foreach (Word word in words.Where(word => word.IsVisible || LessonSettings.AllWords))
            {
                BeginWordsList.Add((Word)word.Clone());
            }
            BeginWordsList = BeginWordsList.Shuffle();
            foreach (Word word in BeginWordsList)
            {
                WordQueue.Enqueue(word);
            }
        }
    }
}
