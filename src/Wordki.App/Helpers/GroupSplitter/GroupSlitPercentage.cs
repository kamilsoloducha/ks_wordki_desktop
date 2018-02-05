using WordkiModel;
using System.Collections.Generic;

namespace Wordki.Helpers.GroupSplitter
{
    public class GroupSlitPercentage : GroupSplitterBase
    {
        public override IEnumerable<IGroup> Split(IGroup group, int factor)
        {
            if (factor >= 100 || factor <= 0)
            {
                yield break;
            }
            if (group == null || group.Words.Count == 0)
            {
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
