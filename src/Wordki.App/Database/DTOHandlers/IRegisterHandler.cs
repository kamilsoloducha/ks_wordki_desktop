using Oazachaosu.Core.Common;
using WordkiModel;

namespace Wordki.Database
{
    public interface IUserHandler
    {
        IUser Handle(UserDTO userDto);
    }
}