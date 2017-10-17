using Repository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wordki.Models;

namespace Wordki.Database2
{
    public class NHibernateDatabase : IDatabase
    {

        public List<IGroup> Groups { get; private set; }

        private IUserRepository _userRepo;
        private IGroupRepository _groupRepo;
        private IWordRepository _wordRepo;
        private IResultRepository _resultRepo;

        public NHibernateDatabase()
        {
            _userRepo = new UserRepository();
            _groupRepo = new GroupRepository();
            _wordRepo = new WordRepository();
            _resultRepo = new ResultRepository();
            Groups = new List<IGroup>();
        }

        public async Task LoadDatabaseAsync()
        {
            if (Groups.Count > 0)
            {
                Groups.Clear();
            }
            foreach (var group in await _groupRepo.GetGroupsAsync())
            {
                Groups.Add(group);
            }
        }

        public async Task SaveDatabaseAsync()
        {
            await _groupRepo.UpdateAsync(Groups);
        }

        public async Task RefreshDatabaseAsync()
        {

        }

        #region User

        public async Task<bool> AddUserAsync(IUser user)
        {
            try
            {
                await _userRepo.SaveAsync(user);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<IUser> GetUserAsync(string name, string password)
        {
            IUser result = null;
            try
            {
                result = await _userRepo.GetAsync(name, password);
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public async Task<bool> UpdateUserAsync(IUser user)
        {
            try
            {
                await _userRepo.UpdateAsync(user);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Groups

        public async Task<bool> AddGroupAsync(IGroup group)
        {
            Groups.Add(group);
            try
            {
                await _groupRepo.SaveAsync(group);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateGroupAsync(IGroup group)
        {
            try
            {
                await _groupRepo.UpdateAsync(group);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteGroupAsync(IGroup group)
        {
            try
            {
                await _groupRepo.DeleteAsync(group);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Word

        public async Task<bool> AddWordAsync(IWord word)
        {
            try
            {
                await _wordRepo.SaveAsync(word);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateWordAsync(IWord word)
        {
            try
            {
                await _wordRepo.UpdateAsync(word);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteWordAsync(IWord word)
        {
            try
            {
                await _wordRepo.DeleteAsync(word);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Result

        public async Task<bool> AddResultAsync(IResult result)
        {
            try
            {
                await _resultRepo.SaveAsync(result);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateResultAsync(IResult result)
        {
            try
            {
                await _resultRepo.UpdateAsync(result);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteResultAsync(IResult result)
        {
            try
            {
                await _resultRepo.DeleteAsync(result);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

    }
}
