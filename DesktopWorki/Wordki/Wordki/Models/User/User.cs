using System;
using System.ComponentModel;
using Newtonsoft.Json;
using WordkiModel.Enums;
using Wordki.Helpers.JsonConverters;
using WordkiModel;

namespace Wordki.Models
{
    [Serializable]
    public class User : ModelBase<IUser>, IUser
    {

        #region Properties
        public virtual string Name { get; set; }

        public virtual string Password { get; set; }

        [JsonProperty("LocalId")]
        public virtual long LocalId { get; set; }

        public virtual bool IsLogin { get; set; }

        public virtual bool IsRegister { get; set; }

        [JsonProperty("lastLoginDate", NullValueHandling = NullValueHandling.Ignore)]
        public virtual DateTime LastLoginDateTime { get; set; }

        public virtual DateTime DownloadTime { get; set; }

        private TranslationDirection translationDirection;
        [DefaultValue(TranslationDirection.FromFirst)]
        [JsonProperty("translationDirection", NullValueHandling = NullValueHandling.Ignore)]
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
        [JsonConverter(typeof(StringToBoolConverter))]
        [DefaultValue(true)]
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
        public virtual bool IsAdmin { get { return false; } set { } }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public User()
        {
            LocalId = 0;
            Name = "";
            Password = "";
            IsLogin = false;
            IsRegister = false;
            LastLoginDateTime = new DateTime();
            DownloadTime = new DateTime(1990, 9, 24);
            TranslationDirection = TranslationDirection.FromSecond;
            AllWords = false;
            ApiKey = "";
        }

    }
}
