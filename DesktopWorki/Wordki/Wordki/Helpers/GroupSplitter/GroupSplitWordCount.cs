using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Helpers.GroupSplitter
{
    public class GroupSplitWordCount : GroupSplitterBase
    {

        public override IEnumerable<Group> Split(Group group, int factor)
        {
            if (group == null || group.WordsList.Count == 0)
            {
                Logger.LogError("Bład pozialu grupy - nie ma nic do podzielenia");
                yield break;
            }
            if (factor >= group.WordsList.Count || factor <= 0)
            {
                Logger.LogError($"Blad podzialu grupy - {factor}");
                yield break;
            }
            List<Word> lWords = new List<Word>(group.WordsList);
            int lNewGroupCount = lWords.Count / factor;
            for (int i = 0; i < lNewGroupCount - 1; i++)
            {
                Group lNewGroup = CreateGroup(group, i + 1);
                int j = group.WordsList.Count - 1;
                while (lNewGroup.WordsList.Count < factor)
                {
                    TransferWord(group.WordsList[j], lNewGroup);
                    j--;
                }
                yield return lNewGroup;
                if (group.WordsList.Count <= factor)
                {
                    break;
                }
            }
        }
    }
}
