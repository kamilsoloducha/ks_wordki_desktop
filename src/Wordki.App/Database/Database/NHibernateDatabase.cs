using WordkiModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Wordki.Database.Repositories;

namespace Wordki.Database
{
    public class NHibernateDatabase : IDatabase
    {

        public IList<IGroup> Groups { get; private set; }

        private IUserRepository _userRepo;
        private IGroupRepository _groupRepo;
        private IWordRepository _wordRepo;
        private IResultRepository _resultRepo;

        internal NHibernateDatabase()
        {
            _userRepo = new UserRepository();
            _groupRepo = new GroupRepository();
            _wordRepo = new WordRepository();
            _resultRepo = new ResultRepository();
            Groups = new ObservableCollection<IGroup>();
        }

        public async Task LoadDatabaseAsync()
        {
            if (Groups.Count > 0)
            {
                Groups.Clear();
            }
            foreach (var group in (await _groupRepo.GetAllAsync()).Where(x => x.State > 0))
            {
                if (group.State < 0)
                    continue;
                Groups.Add(group);
                IEnumerable<IWord> words = group.Words.Where(x => x.State > 0).ToArray();
                group.Words.Clear();
                foreach (IWord word in words)
                {
                    group.Words.Add(word);
                }
                IEnumerable<IResult> results = group.Results.Where(x => x.State > 0).ToArray();
                group.Results.Clear();
                foreach (IResult result in results)
                {
                    group.Results.Add(result);
                }
            }
        }

        public void LoadDatabase()
        {
            if (Groups.Count > 0)
            {
                Groups.Clear();
            }
            foreach (var group in (_groupRepo.GetAll()).Where(x => x.State > 0))
            {
                if (group.State < 0)
                    continue;
                Groups.Add(group);
                IEnumerable<IWord> words = group.Words.Where(x => x.State > 0).ToArray();
                group.Words.Clear();
                foreach (IWord word in words)
                {
                    group.Words.Add(word);
                }
                IEnumerable<IResult> results = group.Results.Where(x => x.State > 0).ToArray();
                group.Results.Clear();
                foreach (IResult result in results)
                {
                    group.Results.Add(result);
                }
            }
        }

        public async Task SaveDatabaseAsync()
        {
            await _groupRepo.UpdateAsync(Groups);
        }

        public void SaveDatabase()
        {
            _groupRepo.Update(Groups);
        }

        public async Task RefreshDatabaseAsync()
        {
        }

        public void RefreshDatabase()
        {
            IEnumerable<IGroup> allGroups = _groupRepo.GetAll();
            foreach (IGroup group in allGroups)
            {
                if (group.State < 0)
                {
                    _groupRepo.Delete(group);
                    continue;
                }
                foreach (IWord word in group.Words.Where(x => x.State < 0))
                {
                    _wordRepo.Delete(word);
                }
                foreach (IResult result in group.Results.Where(x => x.State < 0))
                {
                    _resultRepo.Delete(result);
                }
            }
        }

        #region User

        public async Task<bool> AddUserAsync(IUser user)
        {
            try
            {
                await _userRepo.SaveAsync(user);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }

        public bool AddUser(IUser user)
        {
            try
            {
                _userRepo.Save(user);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
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
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return null;
            }
            return result;
        }

        public IUser GetUser(string name, string password)
        {
            IUser result = null;
            try
            {
                result = _userRepo.Get(name, password);
            }
            catch (Exception e )
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
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
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }

        public bool UpdateUser(IUser user)
        {
            try
            {
                _userRepo.Update(user);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
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
                await _groupRepo.SaveAsync(group);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            Groups.Add(group);
            return true;
        }

        public bool AddGroup(IGroup group)
        {
            try
            {
                _groupRepo.Save(group);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            Groups.Add(group);
            return true;
        }

        public async Task<bool> UpdateGroupAsync(IGroup group)
        {
            if (group != null && group.State == 0)
            {
                return true;
            }
            try
            {
                await _groupRepo.UpdateAsync(group);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }

        public bool UpdateGroup(IGroup group)
        {
            if (group != null && group.State == 0)
            {
                return true;
            }
            try
            {
                _groupRepo.Update(group);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteGroupAsync(IGroup group)
        {
            try
            {
                group.State = -1;
                foreach (IWord word in group.Words)
                {
                    word.State = -1;
                }
                foreach (IResult result in group.Results)
                {
                    result.State = -1;
                }
                await UpdateGroupAsync(group);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            Groups.Remove(group);
            return true;
        }


        public bool DeleteGroup(IGroup group)
        {
            try
            {
                group.State = -1;
                foreach (IWord word in group.Words)
                {
                    word.State = -1;
                }
                foreach (IResult result in group.Results)
                {
                    result.State = -1;
                }
                UpdateGroup(group);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            Groups.Remove(group);
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
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }

        public bool AddWord(IWord word)
        {
            try
            {
                _wordRepo.Save(word);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }
        public async Task<bool> UpdateWordAsync(IWord word)
        {
            if (word != null && word.State == 0)
            {
                return true;
            }
            try
            {
                await _wordRepo.UpdateAsync(word);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }

        public bool UpdateWord(IWord word)
        {
            if (word != null && word.State == 0)
            {
                return true;
            }
            try
            {
                _wordRepo.Update(word);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteWordAsync(IWord word)
        {
            try
            {
                word.State = -1;
                await UpdateWordAsync(word);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            word.Group.Words.Remove(word);
            return true;
        }

        public bool DeleteWord(IWord word)
        {
            try
            {
                word.State = -1;
                UpdateWord(word);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            word.Group.Words.Remove(word);
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
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }

        public bool AddResult(IResult result)
        {
            try
            {
                _resultRepo.Save(result);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateResultAsync(IResult result)
        {
            if (result != null && result.State == 0)
            {
                return true;
            }
            try
            {
                await _resultRepo.UpdateAsync(result);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }

        public bool UpdateResult(IResult result)
        {
            if (result != null && result.State == 0)
            {
                return true;
            }
            try
            {
                _resultRepo.Update(result);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteResultAsync(IResult result)
        {
            try
            {
                result.State = -1;
                await UpdateResultAsync(result);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            result.Group.Results.Remove(result);
            return true;
        }

        public bool DeleteResult(IResult result)
        {
            try
            {
                result.State = -1;
                UpdateResult(result);
            }
            catch (Exception e)
            {
                Helpers.LoggerSingleton.LogError($"Error message: '{e.Message}', StackTrace: '{e.StackTrace}'");
                return false;
            }
            result.Group.Results.Remove(result);
            return true;
        }

        #endregion

    }
}
