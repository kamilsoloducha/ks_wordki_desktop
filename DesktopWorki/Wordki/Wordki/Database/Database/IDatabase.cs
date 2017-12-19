using System.Collections.Generic;
using System.Threading.Tasks;
using WordkiModel;

namespace Wordki.Database
{
    public interface IDatabase
    {
        IList<IGroup> Groups { get; }

        Task<IUser> GetUserAsync(string name, string password);
        IUser GetUser(string name, string password);

        Task<bool> AddGroupAsync(IGroup group);
        bool AddGroup(IGroup group);

        Task<bool> AddResultAsync(IResult result);

        bool AddResult(IResult result);

        Task<bool> AddWordAsync(IWord word);
        bool AddWord(IWord word);

        Task<bool> AddUserAsync(IUser user);
        bool AddUser(IUser user);

        Task<bool> DeleteGroupAsync(IGroup group);
        bool DeleteGroup(IGroup group);

        Task<bool> DeleteResultAsync(IResult result);
        bool DeleteResult(IResult result);

        Task<bool> DeleteWordAsync(IWord word);
        bool DeleteWord(IWord word);

        Task LoadDatabaseAsync();
        void LoadDatabase();

        Task SaveDatabaseAsync();
        void SaveDatabase();

        Task RefreshDatabaseAsync();
        void RefreshDatabase();

        Task<bool> UpdateGroupAsync(IGroup group);
        bool UpdateGroup(IGroup group);

        Task<bool> UpdateResultAsync(IResult result);
        bool UpdateResult(IResult result);

        Task<bool> UpdateWordAsync(IWord word);
        bool UpdateWord(IWord word);

        Task<bool> UpdateUserAsync(IUser user);
        bool UpdateUser(IUser user);


    }
}