using System.Linq;
using System.Collections.Generic;
using WordkiModel;

namespace Wordki.Helpers.GroupSplitter
{
    public class GroupSplitWordCount : GroupSplitterBase
    {

        public override IEnumerable<IGroup> Split(IGroup group, int factor)
        {
            if (group == null || group.Words.Count == 0)
            {
                yield break;
            }
            if (factor >= group.Words.Count || factor <= 0)
            {
                yield break;
            }
            int iterator = 0;
            int count = group.Words.Count;
            while (count / factor > 1)
            {
                IGroup lNewGroup = CreateGroup(group, iterator++);
                while (lNewGroup.Words.Count < factor)
                {
                    TransferWord(group.Words.Last(), lNewGroup);
                }
                count = group.Words.Count;
                yield return lNewGroup;
            }
        }
    }
}
