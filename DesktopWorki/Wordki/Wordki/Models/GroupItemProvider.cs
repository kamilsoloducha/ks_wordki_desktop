using DataVirtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models
{
    public class GroupItemProvider : IItemsProvider<GroupItem>
    {

        IList<GroupItem> groupItems;

        public GroupItemProvider(IList<GroupItem> groupItems)
        {
            this.groupItems = groupItems;
        }

        public int FetchCount()
        {
            return groupItems.Count;
        }

        public IList<GroupItem> FetchRange(int startIndex, int count)
        {
            List<GroupItem> list = new List<GroupItem>();

            for (int i = startIndex; i < startIndex + count; i++)
            {
                list.Add(groupItems[i]);
            }

            return list;
        }
    }
}
