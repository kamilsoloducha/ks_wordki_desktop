using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Models.Enums;
using Repository.Models;
using Util.Collections;

namespace Wordki.Models.Lesson
{
    [Serializable]
    public class TypingLesson : Lesson
    {

        protected IEnumerable<IWord> AllWordList { get; set; }

        public TypingLesson(IEnumerable<IWord> pWordsList)
          : base()
        {
            AllWordList = pWordsList;
            IsCorrect = false;
        }

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

        protected override void CreateWordList()
        {
            foreach (Word word in AllWordList.Where(word => word.Visible || LessonSettings.AllWords))
            {
                BeginWordsList.Add((Word)word.Clone());
            }
            BeginWordsList = BeginWordsList.Shuffle();
            foreach (Word word in BeginWordsList)
            {
                WordList.Enqueue(word);
            }
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
                  (short)word.Group.Words.Count(x => !x.Visible),
                  0,
                  LessonSettings.TranslationDirection,
                  (LessonType)Enum.Parse(typeof(LessonType), GetType().Name),
                  DateTime.Now,
                  int.MaxValue));
            }
        }
    }
}
