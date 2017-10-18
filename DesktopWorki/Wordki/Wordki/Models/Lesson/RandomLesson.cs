using Repository.Models;
using System;
using System.Collections.Generic;

namespace Wordki.Models.Lesson {
  [Serializable]
  public class RandomLesson : TypingLesson {
    public RandomLesson(IEnumerable<IWord> pWordsList) : base(pWordsList) {
    }

    protected override void CreateWordList() {
      foreach (Word word in AllWordList) {
        BeginWordsList.Add((IWord)word.Clone());
      }
      foreach (IWord word in BeginWordsList) {
        WordList.Enqueue(word);
      }
    }

    protected override void CreateResultList() {
      ResultList = new List<IResult>();
    }
  }
}
