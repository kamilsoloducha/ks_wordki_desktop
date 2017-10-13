using Repository.Models;
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
        ObservableCollection<IGroup> GroupsList { get; set; }
        Task<bool> AddGroupAsync(IGroup pGroup);
        Task<bool> AddResultAsync(IResult pResult);
        Task<bool> AddResultAsync(IGroup pGroup, IResult pResult);
        bool AddUser(User pUser);
        Task<bool> AddWordAsync(long pGroupId, IWord pWord);
        Task<bool> AddWordAsync(IGroup pGroup, IWord pWord);
        Task<bool> ConnectWords(IList<IWord> items);
        Task<bool> DeleteGroupAsync(IGroup pGroup);
        Task<bool> DeleteResultAsync(IResult result);
        Task<bool> DeleteResultAsync(IGroup group, IResult result);
        bool DeleteUser(User user);
        Task<bool> DeleteWordAsync(IWord word);
        Task<bool> DeleteWordAsync(long pGroupId, IWord pWord);
        Task<bool> DeleteWordAsync(IGroup pGroup, IWord pWord);
        IEnumerable<double> GetCountWordsByDrawer();
        IGroup GetGroupById(long pGroupId);
        List<IGroup> GetGroupsToSend();
        IResult GetLastResult(long pGroupId);
        ICollection<IResult> GetResultsList(long pGroupId);
        List<IResult> GetResultsToSend();
        User GetUser(long pUserId);
        User GetUser(string name, string password);
        List<User> GetUsers();
        List<IWord> GetWordsToSend();
        bool LoadDatabase();
        Task<bool> LoadDatabaseAsync();
        void OnReadCommonGroup(ApiResponse pResponse);
        void OnReadDateTime(ApiResponse pResponse);
        void OnReadGroups(ApiResponse pResponse);
        void OnReadResults(ApiResponse pResponse);
        void OnReadWords(ApiResponse pResponse);
        void RefreshDatabase();
        void SaveDatabase();
        Task<bool> UpdateGroupAsync(IGroup pGroup);
        Task<bool> UpdateResultAsync(IResult result);
        bool UpdateUser(User user);
        Task<bool> UpdateWordAsync(IWord pWord);
    }
}