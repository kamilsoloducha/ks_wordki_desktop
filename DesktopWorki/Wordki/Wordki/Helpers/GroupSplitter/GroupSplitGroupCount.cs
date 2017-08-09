using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Helpers.GroupSplitter {
  public class GroupSplitGroupCount : GroupSplitter {

    public GroupSplitGroupCount(int pNumber, Group pGroup, Database database)
      : base(database) {
      Number = pNumber;
      Group = pGroup;
    }

    public override List<Group> Split() {
      if (Group == null || Group.WordsList.Count == 0) {
        Logger.LogError("Bład pozialu grupy - nie ma nic do podzielenia");
        return null;
      }
      if (Number > Group.WordsList.Count || Number <= 0) {
        Logger.LogError("Blad podzialu grupy - Number = {0}", Number);
        return null;
      }
      List<Word> lWords = new List<Word>(Group.WordsList);
      List<Group> lGroups = new List<Group>();
      int lWordsCount = lWords.Count / Number;
      for (int i = 0; i < Number - 1; ++i) {
        Group lNewGroup = CreateGroup(i + 1);
        int j = Group.WordsList.Count - 1;
        while (lNewGroup.WordsList.Count < lWordsCount) {
          TransferWord(lWords[j], lNewGroup);
          j--;
        }
        lGroups.Add(lNewGroup);
      }
      return lGroups;
    }
  }
}
