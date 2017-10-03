using Newtonsoft.Json;
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

        public ObservableCollection<Group> GroupsList { get; set; }
        public SqliteConnection Db { get; private set; }

        private Database()
        {
            GroupsList = new ObservableCollection<Group>();
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
        public async Task<bool> AddGroupAsync(Group pGroup)
        {
            GroupsList.Add(pGroup);
            if (!(await Db.InsertGroupAsync(pGroup, UserManager.GetInstance().User) > 0))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateGroupAsync(Group pGroup)
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

        public async Task<bool> DeleteGroupAsync(Group pGroup)
        {
            while (pGroup.WordsList.Count > 0)
            {
                if (!(await DeleteWordAsync(pGroup.WordsList.Last())))
                {
                    Logger.LogError("Błąd w usuwaniu słowa");
                }
            }
            while (pGroup.ResultsList.Count > 0)
            {
                if (!(await DeleteResultAsync(pGroup.ResultsList.Last())))
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
        public async Task<bool> AddWordAsync(Group pGroup, Word pWord)
        {
            pWord.GroupId = pGroup.Id;
            Application.Current.Dispatcher.Invoke(() => pGroup.WordsList.Add(pWord));
            if (!(await Db.InsertWordAsync(pWord, UserManager.GetInstance().User) > 0))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddWordAsync(long pGroupId, Word pWord)
        {
            Group lGroup = GetGroupById(pGroupId);
            return await AddWordAsync(lGroup, pWord);
        }

        public async Task<bool> UpdateWordAsync(Word pWord)
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

        public async Task<bool> DeleteWordAsync(Group pGroup, Word pWord)
        {
            pWord.State = int.MinValue;
            Application.Current.Dispatcher.Invoke(() => pGroup.WordsList.Remove(pWord));
            if (!(await Db.UpdateWordAsync(pWord) > 0))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteWordAsync(long pGroupId, Word pWord)
        {
            Group lGroup = GroupsList.FirstOrDefault(x => x.Id == pGroupId);
            if (lGroup == null)
            {
                return false;
            }
            return await DeleteWordAsync(lGroup, pWord);
        }

        public async Task<bool> DeleteWordAsync(Word word)
        {
            Group lGroup = GroupsList.FirstOrDefault(x => x.Id == word.GroupId);
            if (lGroup == null)
            {
                return false;
            }
            return await DeleteWordAsync(lGroup, word);
        }

        //OPERACJE NA WYNIKACH//
        //-------------------------------------------------------

        public async Task<bool> AddResultAsync(Group pGroup, Result pResult)
        {
            Application.Current.Dispatcher.Invoke(() => pGroup.ResultsList.Add(pResult));
            if (!(await Db.InsertResultAsync(pResult, UserManager.GetInstance().User) > 0))
            {
                return false;
            }
            pResult.State = 0;
            return true;
        }

        public async Task<bool> AddResultAsync(Result pResult)
        {
            Group lGroup = GetGroupById(pResult.GroupId);
            return await AddResultAsync(lGroup, pResult);
        }

        public async Task<bool> UpdateResultAsync(Result result)
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

        public async Task<bool> DeleteResultAsync(Result result)
        {
            Group group = GroupsList.FirstOrDefault(x => x.Id == result.GroupId);
            if (group == null)
            {
                return false;
            }
            return await DeleteResultAsync(group, result);
        }

        public async Task<bool> DeleteResultAsync(Group group, Result result)
        {
            result.State = int.MinValue;
            Application.Current.Dispatcher.Invoke(() => group.ResultsList.Remove(result));
            if (!(await Db.UpdateResultAsync(result) > 0))
            {
                return false;
            }
            return true;
        }

        //-------------------------------------------------------
        public Group GetGroupById(long pGroupId)
        {
            return GroupsList.FirstOrDefault(x => x.Id == pGroupId);
        }

        //OPERACJE NA WYNIKACH//
        //-------------------------------------------------------
        public Result GetLastResult(long pGroupId)
        {
            ICollection<Result> lResultList = GetResultsList(pGroupId);
            return lResultList.OrderByDescending(x => x.DateTime).FirstOrDefault();
        }

        public ICollection<Result> GetResultsList(long pGroupId)
        {
            Group lGroup = GroupsList.FirstOrDefault(x => x.Id == pGroupId);
            if (lGroup == null)
                return new Result[0];
            return lGroup.ResultsList;
        }

        //==================================================================================

        public IEnumerable<double> GetCountWordsByDrawer()
        {
            double[] result = { 0, 0, 0, 0, 0 };
            foreach (Word word in GroupsList.SelectMany(group => group.WordsList))
            {
                result[word.Drawer]++;
            }
            return result;
        }

        //==================================================================================
        public async Task<bool> LoadDatabaseAsync()
        {
            GroupsList.Clear();
            foreach (Group lGroup in await Db.SelectGroupListAsync(UserManager.GetInstance().User.UserId))
            {
                GroupsList.Add(lGroup);
                foreach (Word lWord in await Db.SelectWordListByGroupIdAsync(lGroup.Id))
                {
                    lGroup.WordsList.Add(lWord);
                }
                foreach (Result lResult in await Db.SelectResultsListByGroupIdAsync(lGroup.Id))
                {
                    lGroup.ResultsList.Add(lResult);
                }
            }
            return true;
        }

        public bool LoadDatabase()
        {
            GroupsList.Clear();
            foreach (Group lGroup in Db.SelectGroupList(UserManager.GetInstance().User.UserId))
            {
                GroupsList.Add(lGroup);
                foreach (Word lWord in Db.SelectWordListByGroupId(lGroup.Id))
                {
                    lGroup.WordsList.Add(lWord);
                }
                foreach (Result lResult in Db.SelectResultsListByGroupId(lGroup.Id))
                {
                    lGroup.ResultsList.Add(lResult);
                }
            }
            Logger.LogInfo("Ładuje baze danych");
            return true;
        }

        public async void SaveDatabase()
        {
            foreach (Group lGroup in GroupsList)
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
                foreach (Word lWord in lGroup.WordsList)
                {
                    if (lWord.State == 0)
                        continue;
                    lReturn = await Db.UpdateWordAsync(lWord);
                    if (lReturn != 1)
                    {
                        Logger.LogInfo("Błąd wprowadzania do bazy slowa o Id {0}", lWord.Id);
                    }
                }
                foreach (Result lResult in lGroup.ResultsList)
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

        public List<Group> GetGroupsToSend()
        {
            List<Group> lGroups = Db.SelectGroupsToSend(UserManager.GetInstance().User.UserId);
            Logger.LogInfo("Wysyłam {0} grup", lGroups.Count);
            return lGroups;
        }

        public List<Word> GetWordsToSend()
        {
            List<Word> lWords = Db.SelectWordToSend(UserManager.GetInstance().User.UserId);
            Logger.LogInfo("Wysyłam {0} słów", lWords.Count);
            return lWords;
        }

        public List<Result> GetResultsToSend()
        {
            List<Result> lResults = Db.SelectResultToSend(UserManager.GetInstance().User.UserId);
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
            IEnumerable<Group> lList = JsonConvert.DeserializeObject<IEnumerable<Group>>(pResponse.Message, jsonSerializerSettings);
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
                Group group = GroupsList.FirstOrDefault(x => x.Id == lGroup.Id);
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
                Group group = GroupsList.FirstOrDefault(x => x.Id == lResult.GroupId);
                if (group == null)
                {
                    continue;
                }
                Result result = group.ResultsList.FirstOrDefault(x => x.Id == lResult.Id);
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
                Group group = GroupsList.FirstOrDefault(x => x.Id == lWord.GroupId);
                if (group == null)
                {
                    continue;
                }
                Word word = group.WordsList.FirstOrDefault(x => x.Id == lWord.Id);
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

        public async Task<bool> ConnectWords(IList<Word> items)
        {
            if (items == null)
            {
                return true;
            }
            Word lSameWordDataGridItem = items[0];
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
