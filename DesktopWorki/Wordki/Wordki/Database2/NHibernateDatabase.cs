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

        public void LoadDatabase()
        {
            if (Groups.Count > 0)
            {
                Groups.Clear();
            }
            foreach (var group in _groupRepo.GetGroups())
            {
                Groups.Add(group);
            }
        }

        public void SaveDatabase()
        {
            foreach (var group in Groups)
            {
                _groupRepo.Update(group);
            }
        }

        public void RefreshDatabase()
        {

        }

        #region User

        public async Task<bool> AddUserAsync(IUser user)
        {
            try
            {
                await Task.Run(() => _userRepo.Save(user));
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
                result = await Task.Run<IUser>(() => _userRepo.Get(name, password));
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
                await Task.Run(() => _userRepo.Update(user));
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
            try
            {
                await Task.Run(() => _groupRepo.Save(group));
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
                await Task.Run(() => _groupRepo.Update(group));
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
                await Task.Run(() => _groupRepo.Delete(group));
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
                await Task.Run(() => _wordRepo.Save(word));
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
                await Task.Run(() => _wordRepo.Update(word));
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
                await Task.Run(() => _wordRepo.Delete(word));
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
                await Task.Run(() => _resultRepo.Save(result));
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
                await Task.Run(() => _resultRepo.Update(result));
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
                await Task.Run(() => _resultRepo.Delete(result));
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
