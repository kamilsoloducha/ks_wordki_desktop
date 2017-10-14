using System.Collections.Generic;
using Repository.Models;
using Wordki.Models;

namespace Wordki.Database2
{
    public class GroupsSender : IModelSender<IGroup>
    {

        public IGroupRepository GroupRepo { get; set; }

        public GroupsSender()
        {
            GroupRepo = new GroupRepository();
        }

        public IEnumerable<IGroup> GetModelToSend()
        {
            foreach (IGroup group in GroupRepo.GetGroups())
            {
                if (group.State != 0)
                {
                    yield return group;
                }
            }
        }
    }
}
