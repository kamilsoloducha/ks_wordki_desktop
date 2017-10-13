using Repository.Models;
using System.Collections.Generic;
using System.Linq;

namespace Wordki.Helpers.GroupSplitter
{
    public class GroupSplitGroupCount : GroupSplitterBase
    {
        public override IEnumerable<IGroup> Split(IGroup group, int factor)
        {
            if (group == null || group.Words.Count == 0)
            {
                Logger.LogError("Bład pozialu grupy - nie ma nic do podzielenia");
                yield break;
            }
            if (factor > group.Words.Count || factor < 2)
            {
                Logger.LogError($"Blad podzialu grupy - {factor}");
                yield break;
            }
            int lWordsCount = group.Words.Count / factor;
            for (int i = 0; i < factor - 1; ++i)
            {
                IGroup lNewGroup = CreateGroup(group, i + 1);
                while (lNewGroup.Words.Count < lWordsCount)
                {
                    TransferWord(group.Words.Last(), lNewGroup);
                }
                yield return lNewGroup;
            }
        }
    }
}
