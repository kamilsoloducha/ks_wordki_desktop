using System.Collections.Generic;
using Wordki.Models;
using WordkiModel;
using WordkiModel.DTO;

namespace Repository.Model.DTOConverters
{
    public static class GroupConverter
    {

        public static IGroup GetGroupFromDTO(GroupDTO group)
        {
            return new Group()
            {
                Id = group.Id,
                Name = group.Name,
                Language1 = group.Language1,
                Language2 = group.Language2,
                State = group.State,
                CreationDate = group.CreationDate,
            };
        }

        public static GroupDTO GetDTOFromModel(IGroup group)
        {
            return new GroupDTO()
            {
                Id = group.Id,
                Name = group.Name,
                Language1 = group.Language1,
                Language2 = group.Language2,
                State = group.State,
                CreationDate = group.CreationDate,
            };
        }

        public static IEnumerable<IGroup> GetGroupsFromDTOs(IEnumerable<GroupDTO> groups)
        {
            foreach(GroupDTO group in groups)
            {
                yield return GetGroupFromDTO(group);
            }
        }

        public static IEnumerable<GroupDTO> GetDTOsFromGroups(IEnumerable<IGroup> groups)
        {
            foreach (IGroup group in groups)
            {
                yield return GetDTOFromModel(group);
            }
        }

    }
}
