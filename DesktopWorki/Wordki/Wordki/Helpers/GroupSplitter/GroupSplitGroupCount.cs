using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Helpers.GroupSplitter
{
    public class GroupSplitGroupCount : GroupSplitterBase
    {

        public GroupSplitGroupCount(int pNumber, Group pGroup, Database database)
          : base(database)
        {
            Number = pNumber;
            Group = pGroup;
        }

        public override IEnumerable<Group> Split()
        {
            if (Group == null || Group.WordsList.Count == 0)
            {
                Logger.LogError("Bład pozialu grupy - nie ma nic do podzielenia");
                yield break;
            }
            if (Number > Group.WordsList.Count || Number <= 0)
            {
                Logger.LogError("Blad podzialu grupy - Number = {0}", Number);
                yield break;
            }
            List<Word> lWords = new List<Word>(Group.WordsList);
            List<Group> lGroups = new List<Group>();
            int lWordsCount = lWords.Count / Number;
            for (int i = 0; i < Number - 1; ++i)
            {
                Group lNewGroup = CreateGroup(i + 1);
                int j = Group.WordsList.Count - 1;
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
