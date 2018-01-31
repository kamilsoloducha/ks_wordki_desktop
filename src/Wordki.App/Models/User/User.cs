using System;
using Oazachaosu.Core.Common;
using WordkiModel;

namespace Wordki.Models
{
    [Serializable]
    public class User : ModelBase<IUser>, IUser
    {

        #region Properties

        public virtual string Name { get; set; }

        public virtual string Password { get; set; }

        public virtual long Id { get; set; }

        public virtual bool IsRegister { get; set; }

        public virtual DateTime DownloadTime { get; set; }

        private TranslationDirection translationDirection;
        public virtual TranslationDirection TranslationDirection
        {
            get { return translationDirection; }
            set
            {
                if (translationDirection == value)
                {
                    return;
                }
                translationDirection = value;
                OnPropertyChanged();
            }
        }

        private bool allWords;
        public virtual bool AllWords
        {
            get { return allWords; }
            set
            {
                if (allWords == value)
                {
                    return;
                }
                allWords = value;
                OnPropertyChanged();
            }
        }

        public virtual string ApiKey { get; set; }
        public virtual DateTime CreateDateTime { get { return DateTime.Now; } set {  } }

        #endregion

        public User()
        {
            Id = 0;
            Name = "";
            Password = "";
            IsRegister = false;
            DownloadTime = new DateTime(1990, 9, 24);
            TranslationDirection = TranslationDirection.FromSecond;
            AllWords = false;
            ApiKey = "";
        }

    }
}
