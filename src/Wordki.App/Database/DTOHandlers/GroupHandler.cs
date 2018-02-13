using AutoMapper;
using Oazachaosu.Core.Common;
using System.Collections.Generic;
using System.Linq;
using Wordki.Database.Repositories;
using Wordki.Models;
using WordkiModel;

namespace Wordki.Database
{
    public class GroupHandler : IGroupHandler
    {
        private readonly IMapper mapper;
        private readonly IDatabase database;

        private readonly IGroupRepository groupRepository;

        public GroupHandler(IMapper mapper, IDatabase database)
        {
            this.mapper = mapper;
            this.database = database;
            this.groupRepository = new GroupRepository();
        }

        public void Handle(IEnumerable<GroupDTO> groupsDto)
        {
            IEnumerable<IGroup> groups = mapper.Map<IEnumerable<GroupDTO>, IEnumerable<Group>>(groupsDto);
            IDatabase database = DatabaseSingleton.Instance;
            IEnumerable<IGroup> toUpdate = groups.Where(x => database.Groups.Any(y => y.Id == x.Id));
            IEnumerable<IGroup> toAdd = groups.Where(x => !database.Groups.Any(y => y.Id == x.Id));
            groupRepository.Update(toUpdate);
            groupRepository.Save(toAdd);
            //foreach (IGroup group in groups)
            //{
            //    if (group.State < 0)
            //    {
            //        database.DeleteGroup(group);
            //    }
            //    if (database.Groups.Any(x => x.Id == group.Id))
            //    {
            //        database.UpdateGroup(group);
            //    }
            //    else
            //    {
            //        database.AddGroup(group);
            //    }
            //}
        }
    }
}
