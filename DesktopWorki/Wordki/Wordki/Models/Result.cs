using System;
using System.Threading;
using Repository.Models;
using Repository.Models.Enums;

namespace Wordki.Models
{
    [Serializable]
    public class Result : ModelAbs<IResult>, IComparable<IResult>, IResult
    {

        public virtual long Id { get; set; }
        public virtual long UserId { get; set; }

        public virtual IGroup Group { get; set; }
        public virtual long GroupId
        {
            get
            {
                return Group == null ? 0 : Group.Id;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public virtual short Correct { get; set; }
        public virtual short Accepted { get; set; }
        public virtual short Wrong { get; set; }
        public virtual short Invisibilities { get; set; }
        public virtual short TimeCount { get; set; }

        private TranslationDirection _translationDirection;
        public virtual TranslationDirection TranslationDirection
        {
            get { return _translationDirection; }
            set
            {
                if (_translationDirection == value) return;
                _translationDirection = value;
                State = StateManager.NewState(State);
            }
        }

        public virtual bool ShouldSerializeTransationDirection()
        {
            return StateManager.GetState(State, "TransationDirection") > 0;
        }

        public virtual LessonType LessonType { get; set; }

        public virtual DateTime DateTime { get; set; }

        public virtual int State { get; set; }

        public Result()
        {
            Id = DateTime.Now.Ticks;
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

        public Result(long pResultId, long pGroupId, short pCorrect, short pAccepted, short pWrong, short pUnvisibilities, short pTime, TranslationDirection pTranslationDirection, LessonType pLessonType, DateTime pDate, int pState = 0)
        {
            Id = pResultId < 0 ? DateTime.Now.Ticks : pResultId;
            DateTime = pDate;
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

        public virtual int CompareTo(IResult other)
        {
            if (DateTime > other.DateTime)
                return -1;
            if (DateTime < other.DateTime)
                return 1;
            return 0;
        }

        public override bool Equals(object obj)
        {
            IResult result = obj as IResult;
            if (result != null &&
              result.Id == Id)
            {
                return true;
            }
            return true;
        }
    }
}
