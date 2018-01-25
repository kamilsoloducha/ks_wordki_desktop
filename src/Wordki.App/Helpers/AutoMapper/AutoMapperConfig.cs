using AutoMapper;
using Oazachaosu.Core.Common;
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
