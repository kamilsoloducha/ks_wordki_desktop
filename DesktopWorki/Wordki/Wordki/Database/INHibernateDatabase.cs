using System.Collections.Generic;
using System.Threading.Tasks;
using Repository.Models;

namespace Wordki.Database
{
    public interface IDatabase
    {
        IList<IGroup> Groups { get; }

        Task<IUser> GetUserAsync(string name, string password);
        IUser GetUesr(string name, string password);

        Task<bool> AddGroupAsync(IGroup group);
        bool AddGroup(IGroup group);

        Task<bool> AddResultAsync(IResult result);

        Task<bool> AddWordAsync(IWord word);
        bool AddWord(IWord word);

        Task<bool> AddUserAsync(IUser user);
        Task<bool> DeleteGroupAsync(IGroup group);
        Task<bool> DeleteResultAsync(IResult result);
        Task<bool> DeleteWordAsync(IWord word);
        Task LoadDatabaseAsync();
        Task SaveDatabaseAsync();
        Task RefreshDatabaseAsync();
        Task<bool> UpdateGroupAsync(IGroup group);
        Task<bool> UpdateResultAsync(IResult result);
        Task<bool> UpdateWordAsync(IWord word);
        Task<bool> UpdateUserAsync(IUser user);


    }
}