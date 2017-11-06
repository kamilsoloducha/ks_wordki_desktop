using System.Collections.Generic;
using System.Linq;
using Repository.Models;

namespace Wordki.Helpers.GroupConnector
{
    public class GroupConnector : IGroupConnector
    {

        public IGroup DestinationGroup { get; private set; }

        public bool Connect(IList<IGroup> groups)
        {
            if(groups == null)
            {
                return false;
            }
            if (!CheckGroupsCount(groups))
            {
                return false;
            }
            if (!CheckLanguagesType(groups))
            {
                return false;
            }
            IGroup dest = groups[0];
            for (int i = 1; i < groups.Count; i++)
            {
                foreach(IWord word in groups[i].Words)
                {
                    dest.AddWord(word);
                }
                groups[i].Words.Clear();
                foreach (IResult result in groups[i].Results)
                {
                    dest.AddResult(result);
                }
                groups[i].Results.Clear();
            }
            DestinationGroup = dest;
            return true;
        }


        private bool CheckLanguagesType(IEnumerable<IGroup> groups)
        {
            return groups.GroupBy(x => x.Language1).Count() == 1
                && groups.GroupBy(x => x.Language2).Count() == 1;
        }

        private bool CheckGroupsCount(IEnumerable<IGroup> groups)
        {
            return groups.Count() >= 2;
        }
    }
}
