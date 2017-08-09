using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Models.Enums;
using Wordki.Helpers;

namespace Wordki.Models.Lesson {
  [Serializable]
  public class TypingLesson : Lesson {

    protected IEnumerable<Word> AllWordList { get; set; }

    public TypingLesson(IEnumerable<Word> pWordsList)
      : base() {
      AllWordList = pWordsList;
      IsCorrect = false;
    }

    public override void Known() {
      if (Counter++ <= BeginWordsList.Count) {
        Result lResult = ResultList.FirstOrDefault(x => x.GroupId == SelectedWord.GroupId);
        SelectedWord.Drawer++;
        if (IsCorrect) {
          if (lResult != null) {
            lResult.Correct++;
          }
        } else {
          if (lResult != null) {
            lResult.Accepted++;
          }
        }
      }
      NextWord();
    }

    public override void Check() {
      IsCorrect = false;
      IsChecked = true;
      switch (Database.GetDatabase().User.TranslationDirection) {
        case TranslationDirection.FromSecond:
          if (CheckTranslation(Translation, SelectedWord.Language1))
            IsCorrect = true; //jesli poprawne
          break;
        case TranslationDirection.FromFirst:
          if (CheckTranslation(Translation, SelectedWord.Language2))
            IsCorrect = true; //jesli poprawne
          break;
      }
    }

    protected override void CreateWordList() {
      bool allWords = Database.GetDatabase().User.AllWords;
      foreach (Word word in AllWordList.Where(word => word.Visible || allWords)) {
        BeginWordsList.Add((Word)word.Clone());
      }
      BeginWordsList = ListShuffle.Shuffle(BeginWordsList);
      foreach (Word word in BeginWordsList) {
        WordList.Enqueue(word);
      }
    }

    protected override void CreateResultList() {
      ResultList = new List<Result>();
      foreach (Word lWord in BeginWordsList) {
        if (ResultList.Exists(x => x.GroupId == lWord.GroupId)) continue;
        Group lGroup = Database.GetDatabase().GetGroupById(lWord.GroupId);
        ResultList.Add(new Result(-1,
          lWord.GroupId,
          0,
          0,
          0,
          (short)lGroup.WordsList.Count(x => !x.Visible),
          0,
          Database.GetDatabase().User.TranslationDirection,
          (LessonType)Enum.Parse(typeof(LessonType), GetType().Name),
          DateTime.Now,
          int.MaxValue));
      }
    }
  }
}
