using Newtonsoft.Json;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wordki.Helpers;
using Wordki.LocalDatabase;
using Wordki.Models.Connector;

namespace Wordki.Models
{
    public class Database : IDatabase
    {
        private static IDatabase _database;

        public ObservableCollection<IGroup> GroupsList { get; set; }
        public SqliteConnection Db { get; private set; }

        private Database()
        {
            GroupsList = new ObservableCollection<IGroup>();
            Db = new SqliteConnection();
            Db.OpenConnection();
        }

        public static IDatabase GetDatabase()
        {
            return _database ?? (_database = new Database());
        }

        public static void ClearDatabase()
        {
            _database = null;
        }

        //OPERACJE NA USERACH//
        //-------------------------------------------------------
        public User GetUser(string name, string password)
        {
            return Db.SelectUser(name, password);
        }

        public User GetUser(long pUserId)
        {
            return Db.SelectUser(pUserId);
        }

        public bool AddUser(User pUser)
        {
            if (!(Db.InsertUser(pUser) > 0))
            {
                Logger.LogError("Błąd w dodawaniu usera {0}", pUser.GetStringFromObject());
                return false;
            }
            return true;
        }

        public bool UpdateUser(User user)
        {
            if (!(Db.UpdateUser(user) > 0))
            {
                Logger.LogError("Błąd w update usera {0}", user.GetStringFromObject());
                return false;
            }
            return true;
        }

        public List<User> GetUsers()
        {
            return Db.SelectUsers();
        }

        public bool DeleteUser(User user)
        {
            return Db.DeleteUser(user.UserId) > 0;
        }

        //OPERACJE NA GRUPACH//
        //-------------------------------------------------------
        public async Task<bool> AddGroupAsync(IGroup pGroup)
        {
            GroupsList.Add(pGroup);
            if (!(await Db.InsertGroupAsync(pGroup, UserManager.GetInstance().User) > 0))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateGroupAsync(IGroup pGroup)
        {
            if (pGroup == null || pGroup.State == 0)
            {
                return true;
            }
            Logger.LogInfo("Update group");
            if (!(await Db.UpdateGroupAsync(pGroup) > 0))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteGroupAsync(IGroup pGroup)
        {
            while (pGroup.Words.Count > 0)
            {
                if (!(await DeleteWordAsync(pGroup.Words.Last())))
                {
                    Logger.LogError("Błąd w usuwaniu słowa");
                }
            }
            while (pGroup.Results.Count > 0)
            {
                if (!(await DeleteResultAsync(pGroup.Results.Last())))
                {
                    Logger.LogError("Błąd w usuwaniu wyniku");
                }
            }
            pGroup.State = int.MinValue;
            if (!(await Db.UpdateGroupAsync(pGroup) > 0))
            {
                Logger.LogError("Błąd w usuwaniu grupy");
            }
            GroupsList.Remove(pGroup);
            return true;
        }

        //OPERACJE NA SLOWACH//
        //-------------------------------------------------------
        public async Task<bool> AddWordAsync(IGroup pGroup, IWord pWord)
        {
            pWord.Group = pGroup;
            Application.Current.Dispatcher.Invoke(() => pGroup.Words.Add(pWord));
            if (!(await Db.InsertWordAsync(pWord, UserManager.GetInstance().User) > 0))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddWordAsync(long pGroupId, IWord pWord)
        {
            IGroup lGroup = GetGroupById(pGroupId);
            return await AddWordAsync(lGroup, pWord);
        }

        public async Task<bool> UpdateWordAsync(IWord pWord)
        {
            if (pWord == null || pWord.State == 0)
            {
                return true;
            }
            if (!(await Db.UpdateWordAsync(pWord) > 0))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteWordAsync(IGroup pGroup, IWord pWord)
        {
            pWord.State = int.MinValue;
            Application.Current.Dispatcher.Invoke(() => pGroup.Words.Remove(pWord));
            if (!(await Db.UpdateWordAsync(pWord) > 0))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteWordAsync(long pGroupId, IWord pWord)
        {
            IGroup lGroup = GroupsList.FirstOrDefault(x => x.Id == pGroupId);
            if (lGroup == null)
            {
                return false;
            }
            return await DeleteWordAsync(lGroup, pWord);
        }

        public async Task<bool> DeleteWordAsync(IWord word)
        {
            IGroup lGroup = GroupsList.FirstOrDefault(x => x.Id == word.Group.Id);
            if (lGroup == null)
            {
                return false;
            }
            return await DeleteWordAsync(lGroup, word);
        }

        //OPERACJE NA WYNIKACH//
        //-------------------------------------------------------

        public async Task<bool> AddResultAsync(IGroup pGroup, IResult pResult)
        {
            Application.Current.Dispatcher.Invoke(() => pGroup.Results.Add(pResult));
            if (!(await Db.InsertResultAsync(pResult, UserManager.GetInstance().User) > 0))
            {
                return false;
            }
            pResult.State = 0;
            return true;
        }

        public async Task<bool> AddResultAsync(IResult pResult)
        {
            IGroup lGroup = GetGroupById(pResult.Group.Id);
            return await AddResultAsync(lGroup, pResult);
        }

        public async Task<bool> UpdateResultAsync(IResult result)
        {
            if (result == null || result.State == 0)
            {
                return true;
            }
            if (!(await Db.UpdateResultAsync(result) > 0))
            {
                return false;
            }
            result.State = 0;
            return true;
        }

        public async Task<bool> DeleteResultAsync(IResult result)
        {
            IGroup group = GroupsList.FirstOrDefault(x => x.Id == result.Group.Id);
            if (group == null)
            {
                return false;
            }
            return await DeleteResultAsync(group, result);
        }

        public async Task<bool> DeleteResultAsync(IGroup group, IResult result)
        {
            result.State = int.MinValue;
            Application.Current.Dispatcher.Invoke(() => group.Results.Remove(result));
            if (!(await Db.UpdateResultAsync(result) > 0))
            {
                return false;
            }
            return true;
        }

        //-------------------------------------------------------
        public IGroup GetGroupById(long pGroupId)
        {
            return GroupsList.FirstOrDefault(x => x.Id == pGroupId);
        }

        //OPERACJE NA WYNIKACH//
        //-------------------------------------------------------
        public IResult GetLastResult(long pGroupId)
        {
            ICollection<IResult> lResultList = GetResultsList(pGroupId);
            return lResultList.OrderByDescending(x => x.DateTime).FirstOrDefault();
        }

        public ICollection<IResult> GetResultsList(long pGroupId)
        {
            IGroup lGroup = GroupsList.FirstOrDefault(x => x.Id == pGroupId);
            if (lGroup == null)
                return new Result[0];
            return lGroup.Results;
        }

        //==================================================================================

        public IEnumerable<double> GetCountWordsByDrawer()
        {
            double[] result = { 0, 0, 0, 0, 0 };
            foreach (IWord word in GroupsList.SelectMany(group => group.Words))
            {
                result[word.Drawer]++;
            }
            return result;
        }

        //==================================================================================
        public async Task<bool> LoadDatabaseAsync()
        {
            GroupsList.Clear();
            foreach (IGroup lGroup in await Db.SelectGroupListAsync(UserManager.GetInstance().User.UserId))
            {
                GroupsList.Add(lGroup);
                foreach (IWord lWord in await Db.SelectWordListByGroupIdAsync(lGroup.Id))
                {
                    lGroup.Words.Add(lWord);
                }
                foreach (IResult lResult in await Db.SelectResultsListByGroupIdAsync(lGroup.Id))
                {
                    lGroup.Results.Add(lResult);
                }
            }
            return true;
        }

        public bool LoadDatabase()
        {
            GroupsList.Clear();
            foreach (IGroup lGroup in Db.SelectGroupList(UserManager.GetInstance().User.UserId))
            {
                GroupsList.Add(lGroup);
                foreach (IWord lWord in Db.SelectWordListByGroupId(lGroup.Id))
                {
                    lGroup.Words.Add(lWord);
                }
                foreach (IResult lResult in Db.SelectResultsListByGroupId(lGroup.Id))
                {
                    lGroup.Results.Add(lResult);
                }
            }
            Logger.LogInfo("Ładuje baze danych");
            return true;
        }

        public async void SaveDatabase()
        {
            foreach (IGroup lGroup in GroupsList)
            {
                int lReturn;
                if (lGroup.State != 0)
                {
                    lReturn = await Db.UpdateGroupAsync(lGroup);
                    if (lReturn != 1)
                    {
                        Logger.LogInfo("Błąd wprowadzania do bazy grupy o Id {0}", lGroup.Id);
                    }
                }
                foreach (IWord lWord in lGroup.Words)
                {
                    if (lWord.State == 0)
                        continue;
                    lReturn = await Db.UpdateWordAsync(lWord);
                    if (lReturn != 1)
                    {
                        Logger.LogInfo("Błąd wprowadzania do bazy slowa o Id {0}", lWord.Id);
                    }
                }
                foreach (IResult lResult in lGroup.Results)
                {
                    if (lResult.State == 0)
                        continue;
                    lReturn = await Db.UpdateResultAsync(lResult);
                    if (lReturn != 1)
                    {
                        Logger.LogInfo("Błąd wprowadzania do bazy wyniku o Id {0}", lResult.Id);
                    }
                }
            }
            Logger.LogInfo("Koniec zapisu");
        }

        public List<IGroup> GetGroupsToSend()
        {
            List<IGroup> lGroups = Db.SelectGroupsToSend(UserManager.GetInstance().User.UserId);
            Logger.LogInfo("Wysyłam {0} grup", lGroups.Count);
            return lGroups;
        }

        public List<IWord> GetWordsToSend()
        {
            List<IWord> lWords = Db.SelectWordToSend(UserManager.GetInstance().User.UserId);
            Logger.LogInfo("Wysyłam {0} słów", lWords.Count);
            return lWords;
        }

        public List<IResult> GetResultsToSend()
        {
            List<IResult> lResults = Db.SelectResultToSend(UserManager.GetInstance().User.UserId);
            Logger.LogInfo("Wysyłam {0} grup", lResults.Count);
            return lResults;
        }

        public void RefreshDatabase()
        {
            Logger.LogInfo("odświeżam baze danych");
            Db.RefreshDatabase(UserManager.GetInstance().User);
        }

        public async void OnReadGroups(ApiResponse pResponse)
        {
            if (pResponse.IsError)
            {
                return;
            }
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            IEnumerable<IGroup> lList = JsonConvert.DeserializeObject<IEnumerable<IGroup>>(pResponse.Message, jsonSerializerSettings);
            if (lList == null)
            {
                return;
            }
            foreach (var lGroup in lList)
            {
                if (lGroup.State < 0)
                {
                    await Db.DeleteWordByGroupId(lGroup.Id);
                    Db.DeleteResultByGroupId(lGroup.Id);
                    Db.DeleteGroup(lGroup.Id);
                    continue;
                }
                IGroup group = GroupsList.FirstOrDefault(x => x.Id == lGroup.Id);
                if (group != null)
                {
                    if (lGroup.Equals(group))
                    {
                    }
                    else
                    {
                        lGroup.State = 0;
                        await UpdateGroupAsync(lGroup);
                    }
                }
                else
                {
                    lGroup.State = 0;
                    await AddGroupAsync(lGroup);
                }
            }
        }

        public async void OnReadResults(ApiResponse pResponse)
        {
            if (pResponse.IsError)
            {
                return;
            }
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            IEnumerable<Result> lList = JsonConvert.DeserializeObject<IEnumerable<Result>>(pResponse.Message, jsonSerializerSettings);
            if (lList == null)
            {
                return;
            }
            foreach (Result lResult in lList)
            {
                if (lResult.State < 0)
                {
                    Db.DeleteResult(lResult.Id);
                    continue;
                }
                IGroup group = GroupsList.FirstOrDefault(x => x.Id == lResult.GroupId);
                if (group == null)
                {
                    continue;
                }
                IResult result = group.Results.FirstOrDefault(x => x.Id == lResult.Id);
                if (result != null)
                {
                    if (lResult.Equals(result))
                    {
                        continue;
                    }
                    lResult.State = 0;
                    await UpdateResultAsync(lResult);
                }
                else
                {
                    lResult.State = 0;
                    await AddResultAsync(group, lResult);
                }
            }
        }

        public async void OnReadWords(ApiResponse pResponse)
        {
            if (pResponse.IsError)
            {
                return;
            }
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            var lList = JsonConvert.DeserializeObject<IEnumerable<Word>>(pResponse.Message, jsonSerializerSettings);
            if (lList == null)
            {
                return;
            }
            foreach (Word lWord in lList)
            {
                if (lWord.State < 0)
                {
                    Db.DeleteResult(lWord.Id);
                    continue;
                }
                IGroup group = GroupsList.FirstOrDefault(x => x.Id == lWord.GroupId);
                if (group == null)
                {
                    continue;
                }
                IWord word = group.Words.FirstOrDefault(x => x.Id == lWord.Id);
                if (word != null)
                {
                    if (lWord.Equals(word))
                    {
                        continue;
                    }
                    lWord.State = 0;
                    await UpdateWordAsync(lWord);
                }
                else
                {
                    lWord.State = 0;
                    await AddWordAsync(group, lWord);
                }
            }
        }

        public void OnReadDateTime(ApiResponse pResponse)
        {
            if (pResponse.IsError)
            {
                return;
            }
            if (pResponse.Message == null)
            {
                return;
            }
            string lDateTime = pResponse.Message;
            UserManager.GetInstance().User.DownloadTime = DateTime.Parse(lDateTime);
            UpdateUser(UserManager.GetInstance().User);
        }

        public void OnReadCommonGroup(ApiResponse pResponse)
        {

        }

        public async Task<bool> ConnectWords(IList<IWord> items)
        {
            if (items == null)
            {
                return true;
            }
            IWord lSameWordDataGridItem = items[0];
            StringBuilder lLanguage1 = new StringBuilder();
            StringBuilder lLanguage2 = new StringBuilder();
            StringBuilder lLanguage1Comment = new StringBuilder();
            StringBuilder lLanguage2Comment = new StringBuilder();
            foreach (Word lItem in items)
            {
                if (!lLanguage1.ToString().Contains(lItem.Language1))
                {
                    lLanguage1.Append(lItem.Language1);
                    lLanguage1.Append(", ");
                }
                if (!lLanguage2.ToString().Contains(lItem.Language2))
                {
                    lLanguage2.Append(lItem.Language2);
                    lLanguage2.Append(", ");
                }
                if (!lLanguage1Comment.ToString().Contains(lItem.Language1Comment))
                {
                    lLanguage1Comment.Append(lItem.Language1Comment);
                    lLanguage1Comment.Append(". ");
                }
                if (!lLanguage2Comment.ToString().Contains(lItem.Language2Comment))
                {
                    lLanguage2Comment.Append(lItem.Language2Comment);
                    lLanguage2Comment.Append(". ");
                }
            }
            lLanguage1.Remove(lLanguage1.Length - 2, 2);
            lLanguage2.Remove(lLanguage2.Length - 2, 2);
            lSameWordDataGridItem.Language1 = lLanguage1.ToString();
            lSameWordDataGridItem.Language2 = lLanguage2.ToString();
            lSameWordDataGridItem.Language1Comment = lLanguage1Comment.ToString();
            lSameWordDataGridItem.Language2Comment = lLanguage2Comment.ToString();
            lSameWordDataGridItem.Visible = true;
            lSameWordDataGridItem.Drawer = 0;
            await UpdateWordAsync(lSameWordDataGridItem);
            for (int i = 1; i < items.Count; i++)
            {
                await DeleteWordAsync(items[i]);
            }
            return true;
        }
    }
}
