﻿using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Repository.Models.Enums;

namespace WordkiRepository.Model
{
    [Serializable]
    public class User
    {

        #region Properties
        public string Name { get; set; }
        public string Password { get; set; }
        [JsonProperty("LocalId")]
        public long UserId { get; set; }
        public bool IsLogin { get; set; }
        public bool IsRegister { get; set; }

        [JsonProperty("lastLoginDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastLoginTime { get; set; }
        public DateTime DownloadTime { get; set; }

        [DefaultValue(TranslationDirection.FromFirst)]
        [JsonProperty("translationDirection", NullValueHandling = NullValueHandling.Ignore)]
        public TranslationDirection TranslationDirection { get; set; }

        [JsonConverter(typeof(JsonConverters.StringToBoolConverter))]
        [DefaultValue(true)]
        public bool AllWords { get; set; }

        [DefaultValue(0)]
        [JsonProperty("timeOut", NullValueHandling = NullValueHandling.Ignore)]
        public int Timeout { get; set; }
        public string ApiKey { get; set; }

        #endregion

        /// <summary>
        /// Prywatny konstruktor
        /// </summary>
        public User()
        {
            UserId = 0;
            Name = "";
            Password = "";
            IsLogin = false;
            IsRegister = false;
            LastLoginTime = new DateTime();
            DownloadTime = new DateTime(1990, 9, 24);
            TranslationDirection = TranslationDirection.FromSecond;
            AllWords = false;
            Timeout = 0;
            ApiKey = "";
        }

        public string GetStringFromObject()
        {
            StringBuilder lBuilder = new StringBuilder();
            lBuilder
              .Append("UserId: ").Append(UserId).Append(";")
              .Append("Name: ").Append(Name).Append(";")
              .Append("Password: ").Append(Password).Append(";")
              .Append("IsRegister: ").Append(IsRegister);
            return lBuilder.ToString();
        }
    }
}
