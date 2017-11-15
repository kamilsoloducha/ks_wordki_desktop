using System;
using System.Threading;
using Repository.Models;
using Repository.Models.Enums;

namespace WordkiRepository.Model {
  [Serializable]
  public class Result : ModelAbs<IResult>, IComparable<Result>, IResult {
    
    public long Id { get; set; }
    public long UserId { get; set; }
    public long GroupId { get; set; }
    public short Correct { get; set; }
    public short Accepted { get; set; }
    public short Wrong { get; set; }
    public short Invisibilities { get; set; }
    public short TimeCount { get; set; }

    private TranslationDirection _translationDirection;
    public TranslationDirection TranslationDirection {
      get { return _translationDirection; }
      set {
        if (_translationDirection == value) return;
        _translationDirection = value;
        State = StateManager.NewState(State);
      }
    }

    public bool ShouldSerializeTransationDirection() {
      return StateManager.GetState(State, "TransationDirection") > 0;
    }

    public LessonType LessonType { get; set; }

    public DateTime DateTime { get; set; }

    public int State { get; set; }

    public Result() {
      Id = DateTime.Now.Ticks;
      GroupId = -1;
      Correct = 0;
      Accepted = 0;
      Wrong = 0;
      Invisibilities = 0;
      TimeCount = 0;
      TranslationDirection = TranslationDirection.FromFirst;
      LessonType = LessonType.Unknown;
      DateTime = DateTime.Now;
      State = int.MaxValue;
    }

    public Result(long pResultId, long pGroupId, short pCorrect, short pAccepted, short pWrong, short pUnvisibilities, short pTime, TranslationDirection pTranslationDirection, LessonType pLessonType, DateTime pDate, int pState = 0) {
      Id = pResultId < 0 ? DateTime.Now.Ticks : pResultId;
      DateTime = pDate;
      GroupId = pGroupId;
      Correct = pCorrect;
      Accepted = pAccepted;
      Wrong = pWrong;
      TimeCount = pTime;
      TranslationDirection = pTranslationDirection;
      LessonType = pLessonType;
      Invisibilities = pUnvisibilities;
      State = pState;
      Thread.Sleep(1);
    }

    public int CompareTo(Result other) {
      if (DateTime > other.DateTime)
        return -1;
      if (DateTime < other.DateTime)
        return 1;
      return 0;
    }

    public override bool Equals(object obj) {
      Result result = obj as Result;
      if (result != null &&
        result.Id == Id) {
        return true;
      }
      return true;
    }

    public void SwapDirection() {
      TranslationDirection = TranslationDirection == TranslationDirection.FromFirst ? TranslationDirection.FromSecond : TranslationDirection.FromFirst;
    }
  }
}
