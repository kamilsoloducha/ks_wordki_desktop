using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Helpers.GroupSplitter {
  public class GroupSlitPercentage : GroupSplitter {

    public GroupSlitPercentage(int pNumber, Group pGroup, Database database)
      : base(database) {
      Number = pNumber;
      Group = pGroup;
    }

    public override List<Group> Split() {
      if (Number >= 100 || Number <= 0) {
        Logger.LogError("Blad podzialu grupy - Number = {0}", Number);
        return null;
      }
      if (Group == null || Group.WordsList.Count == 0) {
        Logger.LogError("Bład pozialu grupy - nie ma nic do podzielenia");
        return null;
      }
      Group lNewGroup = CreateGroup(1);
      List<Word> lWords = new List<Word>(Group.WordsList);
      int lWordsCount1 = (Group.WordsList.Count * Number / 100);
      for (int i = lWordsCount1; i < lWords.Count; ++i) {
        TransferWord(lWords[i], lNewGroup);
      }
      List<Group> lGroups = new List<Group>();
      lGroups.Add(lNewGroup);
      return lGroups;
    }
  }
}
