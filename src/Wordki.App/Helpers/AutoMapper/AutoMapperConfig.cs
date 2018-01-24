using AutoMapper;
using Oazachaosu.Core.Common;
using WordkiModel;

namespace Wordki.Helpers.AutoMapper
{
    public static class AutoMapperConfig
    {

        public static IMapper Initialize()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IUser, UserDTO>();
                cfg.CreateMap<UserDTO, IUser>();
                cfg.CreateMap<IGroup, GroupDTO>();
                cfg.CreateMap<GroupDTO, IGroup>();
                cfg.CreateMap<IWord, WordDTO>();
                cfg.CreateMap<WordDTO, IWord>();
                cfg.CreateMap<IResult, ResultDTO>();
                cfg.CreateMap<ResultDTO, IResult>();
            }).CreateMapper();
        }

    }
}
