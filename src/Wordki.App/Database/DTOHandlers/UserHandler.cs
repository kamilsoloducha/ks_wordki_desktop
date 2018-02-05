using System;
using AutoMapper;
using Oazachaosu.Core.Common;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Database
{
    public class UserHandler : IUserHandler
    {

        private readonly IMapper mapper;
        private readonly IDatabaseOrganizer databaseOrganizer;

        public UserHandler(IMapper mapper, IDatabaseOrganizer databaseOrganizer)
        {
            this.mapper = mapper;
            this.databaseOrganizer = databaseOrganizer;
        }

        public IUser Handle(UserDTO userDto)
        {
            IUser user = mapper.Map<UserDTO, User>(userDto);
            user.IsRegister = true;
            NHibernateHelper.DatabaseName = user.Name;
            if (!databaseOrganizer.AddDatabase(user))
            {
                Console.WriteLine("Błąd dodawania bazy danych");
                return null;
            }
            return user;
        }
    }
}
