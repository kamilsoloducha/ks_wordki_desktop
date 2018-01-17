using System;
using System.Collections.Generic;
using System.Linq;
using WordkiModel.Enums;
using WordkiModel;
using Util.Collections;

namespace Wordki.Models.Lesson
{
    [Serializable]
    public class TypingLesson : Lesson
    {

        public TypingLesson() : base() { }

        public override void Known()
        {
            if (Counter++ <= BeginWordsList.Count)
            {
                IResult lResult = ResultList.FirstOrDefault(x => x.Group.Id == SelectedWord.Group.Id);
                SelectedWord.Drawer++;
                if (IsCorrect)
                {
                    if (lResult != null)
                    {
                        lResult.Correct++;
                    }
                }
                else
                {
                    if (lResult != null)
                    {
                        lResult.Accepted++;
                    }
                }
            }
            NextWord();
        }

        public override void Check(string translation)
        {
            IsCorrect = false;
            IsChecked = true;
            switch (LessonSettings.TranslationDirection)
            {
                case TranslationDirection.FromSecond:
                    if (CheckTranslation(translation, SelectedWord.Language1))
                        IsCorrect = true; //jesli poprawne
                    break;
                case TranslationDirection.FromFirst:
                    if (CheckTranslation(translation, SelectedWord.Language2))
                        IsCorrect = true; //jesli poprawne
                    break;
            }
        }

        protected override void CreateWordList(IEnumerable<IWord> words)
        {
            BeginWordsList.AddRange(words.Where(word => word.IsVisible || LessonSettings.AllWords).Select(x => (IWord)x.Clone()));
            BeginWordsList.Shuffle();
            foreach (IWord word in BeginWordsList)
            {
                WordQueue.Enqueue(word);
            }
        }

        protected override void CreateResultList()
        {
            ResultList.AddRange(BeginWordsList.GroupBy(x => x.Group).Select(x =>
            new Result(-1,
                  x.Key,
                  0,
                  0,
                  0,
                  (short)x.Key.Words.Count(y => !y.IsVisible),
                  0,
                  LessonSettings.TranslationDirection,
                  (LessonType)Enum.Parse(typeof(LessonType), GetType().Name),
                  DateTime.Now,
                  int.MaxValue)));
        }
    }
}
