using AutoMapper;
using Oazachaosu.Core.Common;
using System.Collections.Generic;
using System.Linq;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Database
{
    public class GroupHandler : IGroupHandler
    {
        private readonly IMapper mapper;
        private readonly IDatabase database;

        public GroupHandler(IMapper mapper, IDatabase database)
        {
            this.mapper = mapper;
            this.database = database;
        }

        public void Handle(IEnumerable<GroupDTO> groupsDto)
        {
            IEnumerable<IGroup> groups = mapper.Map<IEnumerable<GroupDTO>, IEnumerable<Group>>(groupsDto);
            IDatabase database = DatabaseSingleton.Instance;
            foreach (IGroup group in groups)
            {
                if (group.State < 0)
                {
                    database.DeleteGroup(group);
                }
                if (database.Groups.Any(x => x.Id == group.Id))
                {
                    database.UpdateGroup(group);
                }
                else
                {
                    database.AddGroup(group);
                }
            }
        }
    }
}
