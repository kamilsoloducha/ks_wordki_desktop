using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Models.Enums;
using Repository.Models;
using Wordki.Database2;

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
                Result lResult = ResultList.FirstOrDefault(x => x.GroupId == SelectedWord.GroupId);
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
            switch (UserManagerSingleton.Get().User.TranslationDirection)
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
            bool allWords = UserManagerSingleton.Get().User.AllWords;
            foreach (Word word in AllWordList.Where(word => word.Visible || allWords))
            {
                BeginWordsList.Add((Word)word.Clone());
            }
            BeginWordsList = Util.Collections.Utils.Shuffle(BeginWordsList);
            foreach (Word word in BeginWordsList)
            {
                WordList.Enqueue(word);
            }
        }

        protected override void CreateResultList()
        {
            ResultList = new List<Result>();
            foreach (Word lWord in BeginWordsList)
            {
                if (ResultList.Exists(x => x.GroupId == lWord.GroupId)) continue;
                IGroup lGroup = Database.GetDatabase().GetGroupById(lWord.GroupId);
                ResultList.Add(new Result(-1,
                  lWord.GroupId,
                  0,
                  0,
                  0,
                  (short)lGroup.Words.Count(x => !x.Visible),
                  0,
                  UserManagerSingleton.Get().User.TranslationDirection,
                  (LessonType)Enum.Parse(typeof(LessonType), GetType().Name),
                  DateTime.Now,
                  int.MaxValue));
            }
        }
    }
}
