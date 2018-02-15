using System.Collections.Generic;
using System.Threading.Tasks;
using WordkiModel;
using Wordki.Models;
using System.Collections.ObjectModel;
using System;
using Oazachaosu.Core.Common;
using WordkiModel.Extensions;

namespace Wordki.Database
{
    public class MemoryDatabase : IDatabase
    {

        public IList<IGroup> Groups { get; private set; }

        public MemoryDatabase()
        {
            Groups = new ObservableCollection<IGroup>();
        }


        public bool AddGroup(IGroup group)
        {
            Groups.Add(group);
            return true;
        }

        public Task<bool> AddGroupAsync(IGroup group)
        {
            Groups.Add(group);
            return Task.FromResult(true);
        }

        public bool AddResult(IResult result)
        {
            return true;
        }

        public Task<bool> AddResultAsync(IResult result)
        {
            return Task.FromResult(true);
        }

        public bool AddUser(IUser user)
        {
            return true;
        }

        public Task<bool> AddUserAsync(IUser user)
        {
            return Task.FromResult(true);
        }

        public bool AddWord(IWord word)
        {
            return true;
        }

        public Task<bool> AddWordAsync(IWord word)
        {
            return Task.FromResult(true);
        }

        public bool DeleteGroup(IGroup group)
        {
            Groups.Remove(group);
            return true;
        }

        public Task<bool> DeleteGroupAsync(IGroup group)
        {
            Groups.Remove(group);
            return Task.FromResult(true);
        }

        public bool DeleteResult(IResult result)
        {
            result.Group.Results.Remove(result);
            return true;
        }

        public Task<bool> DeleteResultAsync(IResult result)
        {
            result.Group.Results.Remove(result);
            return Task.FromResult(true);
        }

        public bool DeleteWord(IWord word)
        {
            word.Group.Words.Remove(word);
            return true;
        }

        public Task<bool> DeleteWordAsync(IWord word)
        {
            word.Group.Words.Remove(word);
            return Task.FromResult(true);
        }

        public IUser GetUser(string name, string password)
        {
            IUser user = new User
            {
                Name = name,
                Password = password,
                Id = 1
            };
            return user;
        }

        public Task<IUser> GetUserAsync(string name, string password)
        {
            IUser user = new User
            {
                Name = name,
                Password = password,
                Id = 1
            };
            return Task.FromResult(user);
        }

        public void LoadDatabase()
        {
            Random random = new Random(100);
            Groups.Clear();
            for (int i = 0; i < 10; i++)
            {
                IGroup group = new Group()
                {
                    Name = $"Group {i}",
                    Language1 = LanguageType.Germany,
                    Language2 = LanguageType.Russian,
                };
                for (int j = 0; j < random.Next(5, 10); j++)
                {
                    IResult result = new Result()
                    {
                        Accepted = 10,
                        Correct = 10,
                        Invisible = 10,
                        LessonType = LessonType.TypingLesson,
                        TimeCount = 300,
                        TranslationDirection = TranslationDirection.FromSecond,
                        Wrong = 10,
                    };
                    group.AddResult(result);
                }

                for (int j = 0; j < random.Next(3, 4); j++)
                {
                    IWord word = new Word()
                    {
                        Language1 = $"Słowo {i}",
                        Language2 = $"Word {i}",
                        Language1Comment = $"Komentarz {i}",
                        Language2Comment = $"Comment {i}",
                        Drawer = (byte)random.Next(4),
                        IsSelected = false,
                        IsVisible = true
                    };
                    group.AddWord(word);
                }
                Groups.Add(group);
            }
        }

        public Task LoadDatabaseAsync()
        {
            return Task.Run(() => LoadDatabase());
        }

        public void RefreshDatabase()
        {

        }

        public Task RefreshDatabaseAsync()
        {
            return Task.FromResult(0);
        }

        public void SaveDatabase()
        {

        }

        public Task SaveDatabaseAsync()
        {
            return Task.FromResult(0);
        }

        public bool UpdateGroup(IGroup group)
        {
            return true;
        }

        public Task<bool> UpdateGroupAsync(IGroup group)
        {
            return Task.FromResult(true);
        }

        public bool UpdateResult(IResult result)
        {
            return true;
        }

        public Task<bool> UpdateResultAsync(IResult result)
        {
            return Task.FromResult(true);
        }

        public bool UpdateUser(IUser user)
        {
            return true;
        }

        public Task<bool> UpdateUserAsync(IUser user)
        {
            return Task.FromResult(true);
        }

        public bool UpdateWord(IWord word)
        {
            return true;
        }

        public Task<bool> UpdateWordAsync(IWord word)
        {
            return Task.FromResult(true);
        }
    }
}
