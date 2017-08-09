using System;
using System.Collections.Generic;

namespace Wordki.Models.Lesson {
  [Serializable]
  public class RandomLesson : TypingLesson {
    public RandomLesson(IEnumerable<Word> pWordsList) : base(pWordsList) {
    }

    protected override void CreateWordList() {
      foreach (Word word in AllWordList) {
        BeginWordsList.Add((Word)word.Clone());
      }
      foreach (Word word in BeginWordsList) {
        WordList.Enqueue(word);
      }
    }

    protected override void CreateResultList() {
      ResultList = new List<Result>();
    }
  }
}
