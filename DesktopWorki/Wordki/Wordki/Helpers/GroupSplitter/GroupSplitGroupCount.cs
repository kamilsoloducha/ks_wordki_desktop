using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Helpers.GroupSplitter
{
    public class GroupSplitGroupCount : GroupSplitterBase
    {
        public override IEnumerable<Group> Split(Group group, int factor)
        {
            if (group == null || group.WordsList.Count == 0)
            {
                Logger.LogError("Bład pozialu grupy - nie ma nic do podzielenia");
                yield break;
            }
            if (factor > group.WordsList.Count || factor <= 0)
            {
                Logger.LogError($"Blad podzialu grupy - {factor}");
                yield break;
            }
            List<Word> lWords = new List<Word>(group.WordsList);
            List<Group> lGroups = new List<Group>();
            int lWordsCount = lWords.Count / factor;
            for (int i = 0; i < factor - 1; ++i)
            {
                Group lNewGroup = CreateGroup(group, i + 1);
                int j = group.WordsList.Count - 1;
                while (lNewGroup.WordsList.Count < lWordsCount)
                {
                    TransferWord(lWords[j], lNewGroup);
                    j--;
                }
                yield return lNewGroup;
            }
        }
    }
}
