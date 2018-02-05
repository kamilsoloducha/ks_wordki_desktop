using AutoMapper;
using Oazachaosu.Core.Common;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Helpers.AutoMapper
{
    public static class AutoMapperConfig
    {

        private static IMapper instance;
        private static object lockObj = new object();

        public static IMapper Instance
        {
            get
            {
                lock (lockObj)
                {
                    if(instance == null)
                    {
                        instance = Initialize();
                    }
                }
                return instance;
            }
        }

        private static IMapper Initialize()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IUser, UserDTO>();
                cfg.CreateMap<IGroup, GroupDTO>();
                cfg.CreateMap<IWord, WordDTO>();
                cfg.CreateMap<IResult, ResultDTO>();
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<Group, GroupDTO>();
                cfg.CreateMap<GroupDTO, Group>();
                cfg.CreateMap<Word, WordDTO>();
                cfg.CreateMap<WordDTO, Word>();
                cfg.CreateMap<Result, ResultDTO>();
                cfg.CreateMap<ResultDTO, Result>();
            }).CreateMapper();
        }

    }
}
