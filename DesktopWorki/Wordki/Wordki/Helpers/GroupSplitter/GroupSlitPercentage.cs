using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Helpers.GroupSplitter
{
    public class GroupSlitPercentage : GroupSplitterBase
    {

        public GroupSlitPercentage(int pNumber, Group pGroup, Database database)
          : base(database)
        {
            Number = pNumber;
            Group = pGroup;
        }

        public override IEnumerable<Group> Split()
        {
            if (Number >= 100 || Number <= 0)
            {
                Logger.LogError("Blad podzialu grupy - Number = {0}", Number);
                yield break;
            }
            if (Group == null || Group.WordsList.Count == 0)
            {
                Logger.LogError("Bład pozialu grupy - nie ma nic do podzielenia");
                yield break;
            }
            Group lNewGroup = CreateGroup(1);
            List<Word> lWords = new List<Word>(Group.WordsList);
            int lWordsCount1 = (Group.WordsList.Count * Number / 100);
            for (int i = lWordsCount1; i < lWords.Count; ++i)
            {
                TransferWord(lWords[i], lNewGroup);
            }
            yield return lNewGroup;
        }
    }
}
