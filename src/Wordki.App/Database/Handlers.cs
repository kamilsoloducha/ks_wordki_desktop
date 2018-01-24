using System.Collections.Generic;
using System.Linq;
using Wordki.Database.Repositories;
using WordkiModel;

namespace Wordki.Database
{
    public class GroupsHandler
    {
        IGroupRepository Repository { get; set; }
        public void Handle(IEnumerable<IGroup> groups)
        {
            IEnumerable<IGroup> groupsFromDatabase = Repository.GetAll();
            foreach(IGroup group in groups)
            {
                if(group.State < 0 && !groupsFromDatabase.Any(x => x.Id == group.Id))
                {
                    continue;
                }
                if(group.State > 0 && !groupsFromDatabase.Any(x => x.Id == group.Id))
                {
                    Repository.Save(group);
                }
                Repository.Update(group);
            }
        }

    }
}
