using System.Collections.Generic;
using WordkiModel;
using Wordki.Models;
using Wordki.Database.Repositories;

namespace Wordki.Database
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
            foreach (IGroup group in GroupRepo.GetAll())
            {
                if (group.State != 0)
                {
                    yield return group;
                }
            }
        }
    }
}
