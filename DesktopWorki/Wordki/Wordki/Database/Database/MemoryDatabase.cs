using System.Collections.Generic;
using System.Threading.Tasks;
using Repository.Models;
using Wordki.Models;

namespace Wordki.Database
{
    public class MemoryDatabase : IDatabase
    {

        public IList<IGroup> Groups { get; private set; }

        public MemoryDatabase()
        {
            Groups = new List<IGroup>();
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
            IUser user = new User();
            user.Name = name;
            user.Password = password;
            user.LocalId = 1;
            return user;
        }

        public Task<IUser> GetUserAsync(string name, string password)
        {
            IUser user = new User();
            user.Name = name;
            user.Password = password;
            user.LocalId = 1;
            return Task.FromResult(user);
        }

        public void LoadDatabase()
        {
            
        }

        public Task LoadDatabaseAsync()
        {
            return Task.FromResult(0);
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
