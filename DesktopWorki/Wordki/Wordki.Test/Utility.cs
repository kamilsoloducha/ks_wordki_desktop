using NUnit.Framework;
using Repository.Models;
using Repository.Models.Enums;
using Repository.Models.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public LanguageType Language1 { get; set; }
        public LanguageType Language2 { get; set; }

        public TranslationDirection Direction { get; set; }

        public Utility()
        {
            GroupCount = 10;
            WordCount = 10;
            ResultCount = 10;
            Language1 = LanguageType.Polish;
            Language2 = LanguageType.English;
            Direction = TranslationDirection.FromSecond;
        }

        public List<IGroup> GetGroups()
        {
            List<IGroup> groups = new List<IGroup>();

            for (int i = 0; i < GroupCount; i++)
            {
                IGroup group = GetGroup();
                groups.Add(group);
            }

            return groups;
        }

        public Word GetWord()
        {
            return new Word()
            {
                Language1 = "Language1",
                Language1Comment = "Language1Comment",
                Language2 = "Language2",
                Language2Comment = "Language2Comment",
                Drawer = 3,
                State = 2,
                Visible = true,
            };
        }

        public Result GetResult()
        {
            return new Result()
            {
                Correct = 10,
                Accepted = 10,
                Wrong = 10,
                Invisibilities = 10,
                DateTime = new DateTime(1990, 9, 24, 12, 0, 0),
                LessonType = Repository.Models.Enums.LessonType.TypingLesson,
                TimeCount = 10,
                TranslationDirection = Direction,
                State = 3,
            };
        }

        public IGroup GetGroup()
        {
            Group group = new Group()
            {
                Id = groupCounter++,
                Language1 = Language1,
                Language2 = Language2,
                Name = "Name",
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
