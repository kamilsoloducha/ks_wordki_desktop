using Repository.Models;
using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Helpers.GroupSplitter
{
    public class GroupSlitPercentage : GroupSplitterBase
    {
        public override IEnumerable<IGroup> Split(IGroup group, int factor)
        {
            if (factor >= 100 || factor <= 0)
            {
                LoggerSingleton.LogError($"Blad podzialu grupy - {factor}");
                yield break;
            }
            if (group == null || group.Words.Count == 0)
            {
                LoggerSingleton.LogError("Bład pozialu grupy - nie ma nic do podzielenia");
                yield break;
            }
            IGroup lNewGroup = CreateGroup(group, 1);
            List<IWord> lWords = new List<IWord>(group.Words);
            int lWordsCount1 = (group.Words.Count * factor / 100);
            for (int i = lWordsCount1; i < lWords.Count; ++i)
            {
                TransferWord(lWords[i], lNewGroup);
            }
            yield return lNewGroup;
        }
    }
}
