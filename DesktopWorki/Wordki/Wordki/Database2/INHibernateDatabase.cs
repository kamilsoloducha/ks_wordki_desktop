using System.Collections.Generic;
using System.Threading.Tasks;
using Repository.Models;

namespace Wordki.Database2
{
    public interface IDatabase
    {
        List<IGroup> Groups { get; }

        Task<IUser> GetUserAsync(string name, string password);
        Task<bool> AddGroupAsync(IGroup group);
        Task<bool> AddResultAsync(IResult result);
        Task<bool> AddWordAsync(IWord word);
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