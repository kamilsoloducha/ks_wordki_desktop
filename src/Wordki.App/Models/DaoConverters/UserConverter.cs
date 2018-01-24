using Wordki.Models;
using WordkiModel;
using WordkiModel.DTO;

namespace Repository.Model.DTOConverters
{
    public static class UserConverter
    {

        public static UserDTO GetDTOFromModel(IUser user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                ApiKey = user.ApiKey,
            };
        }

        public static IUser GetModelFromDTO(UserDTO user)
        {
            return new User()
            {
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                ApiKey = user.ApiKey,
            };
        }
    }
}
