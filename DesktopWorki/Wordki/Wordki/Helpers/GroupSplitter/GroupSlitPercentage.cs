using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Helpers.GroupSplitter
{
    public class GroupSlitPercentage : GroupSplitterBase
    {
        public override IEnumerable<Group> Split(Group group, int factor)
        {
            if (factor >= 100 || factor <= 0)
            {
                Logger.LogError($"Blad podzialu grupy - {factor}");
                yield break;
            }
            if (group == null || group.WordsList.Count == 0)
            {
                Logger.LogError("Bład pozialu grupy - nie ma nic do podzielenia");
                yield break;
            }
            Group lNewGroup = CreateGroup(group, 1);
            List<Word> lWords = new List<Word>(group.WordsList);
            int lWordsCount1 = (group.WordsList.Count * factor / 100);
            for (int i = lWordsCount1; i < lWords.Count; ++i)
            {
                TransferWord(lWords[i], lNewGroup);
            }
            yield return lNewGroup;
        }
    }
}
