using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Wordki.LocalDatabase;
using Wordki.Models.Connector;

namespace Wordki.Models
{
    public interface IDatabase
    {
        SqliteConnection Db { get; }
        ObservableCollection<Group> GroupsList { get; set; }
        Task<bool> AddGroupAsync(Group pGroup);
        Task<bool> AddResultAsync(Result pResult);
        Task<bool> AddResultAsync(Group pGroup, Result pResult);
        bool AddUser(User pUser);
        Task<bool> AddWordAsync(long pGroupId, Word pWord);
        Task<bool> AddWordAsync(Group pGroup, Word pWord);
        Task<bool> ConnectWords(IList<Word> items);
        Task<bool> DeleteGroupAsync(Group pGroup);
        Task<bool> DeleteResultAsync(Result result);
        Task<bool> DeleteResultAsync(Group group, Result result);
        bool DeleteUser(User user);
        Task<bool> DeleteWordAsync(Word word);
        Task<bool> DeleteWordAsync(long pGroupId, Word pWord);
        Task<bool> DeleteWordAsync(Group pGroup, Word pWord);
        IEnumerable<double> GetCountWordsByDrawer();
        Group GetGroupById(long pGroupId);
        List<Group> GetGroupsToSend();
        Result GetLastResult(long pGroupId);
        ICollection<Result> GetResultsList(long pGroupId);
        List<Result> GetResultsToSend();
        User GetUser(long pUserId);
        User GetUser(string name, string password);
        List<User> GetUsers();
        List<Word> GetWordsToSend();
        bool LoadDatabase();
        Task<bool> LoadDatabaseAsync();
        void OnReadCommonGroup(ApiResponse pResponse);
        void OnReadDateTime(ApiResponse pResponse);
        void OnReadGroups(ApiResponse pResponse);
        void OnReadResults(ApiResponse pResponse);
        void OnReadWords(ApiResponse pResponse);
        void RefreshDatabase();
        void SaveDatabase();
        Task<bool> UpdateGroupAsync(Group pGroup);
        Task<bool> UpdateResultAsync(Result result);
        bool UpdateUser(User user);
        Task<bool> UpdateWordAsync(Word pWord);
    }
}