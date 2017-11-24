using NUnit.Framework;
using Repository.Models;
using Repository.Models.Enums;
using Repository.Models.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Wordki.Models;

namespace Wordki.Test
{
    public class Utility
    {

        private static int groupCounter = 1;
        private static int wordCounter = 1;
        private static int resultCounter = 1;

        public int GroupCount { get; set; }
        public int WordCount { get; set; }
        public int ResultCount { get; set; }

        public Utility()
        {
            GroupCount = 10;
            WordCount = 10;
            ResultCount = 10;
        }

        public List<IGroup> GetGroups(int count)
        {
            List<IGroup> groups = new List<IGroup>();

            for (int i = 0; i < count; i++)
            {
                IGroup group = GetGroup();
                groups.Add(group);
            }

            return groups;
        }

        public List<IGroup> GetGroups()
        {
            return GetGroups(GroupCount);
        }

        public Word GetWord(string language1 = "lang1",
            string language2 = "lang2",
            string language1Comment = "lang1Comment",
            string language2Comment = "lang2Comment",
            byte drawer = 3,
            bool visible = true)
        {
            return new Word()
            {
                Language1 = language1,
                Language1Comment = language1Comment,
                Language2 = language2,
                Language2Comment = language2Comment,
                Drawer = drawer,
                State = 2,
                Visible = visible,
            };
        }

        public Result GetResult(short correct = 10,
            short accepted = 10,
            short wrong = 10,
            short invisibilities = 10,
            LessonType lessonType = LessonType.TypingLesson,
            short timeCount = 10,
            TranslationDirection direction = TranslationDirection.FromSecond)
        {
            return new Result()
            {
                Correct = correct,
                Accepted = accepted,
                Wrong = wrong,
                Invisibilities = invisibilities,
                DateTime = new DateTime(1990, 9, 24, 12, 0, 0),
                LessonType = lessonType,
                TimeCount = timeCount,
                TranslationDirection = direction,
                State = 3,
            };
        }

        public IGroup GetGroup(LanguageType language1 = LanguageType.English, LanguageType language2 = LanguageType.Polish, string name = "Name")
        {
            Group group = new Group()
            {
                Id = groupCounter++,
                Language1 = language1,
                Language2 = language2,
                Name = name,
                State = 3,
            };
            for (int i = 0; i < WordCount; i++)
            {
                IWord word = GetWord();
                word.Id = wordCounter++;
                group.AddWord(word);
            }

            for (int i = 0; i < ResultCount; i++)
            {
                IResult result = GetResult();
                result.Id = resultCounter++;
                group.AddResult(result);
            }
            return group;
        }

        public IUser GetUser()
        {
            return new User()
            {
                LocalId = 1,
                AllWords = true,
                ApiKey = "asdffdsaasdffdsa",
                DownloadTime = new DateTime(1990, 9, 24, 12, 0, 0),
                IsLogin = true,
                IsRegister = true,
                LastLoginDateTime = new DateTime(1990, 9, 24, 12, 0, 0),
                Name = "Name",
                Password = "Password",
                Timeout = 10,
                TranslationDirection = TranslationDirection.FromSecond,
            };
        }

        public void CheckUser(IUser expected, IUser actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.ApiKey, actual.ApiKey);
            Assert.AreEqual(expected.AllWords, actual.AllWords);
            Assert.AreEqual(expected.TranslationDirection, actual.TranslationDirection);
            Assert.AreEqual(expected.Timeout, actual.Timeout);
            Assert.AreEqual(expected.LastLoginDateTime, actual.LastLoginDateTime);

        }

    }
}
