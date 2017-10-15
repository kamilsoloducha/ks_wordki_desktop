using Repository.Models;

namespace Wordki.Database2
{
    public interface IUserOrganizer
    {
        bool CheckDatabase(IUser user);
        bool AddDatabase(IUser user);
        bool RemoveDatabase(IUser user);
    }
}
