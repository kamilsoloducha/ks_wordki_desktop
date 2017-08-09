using System.Collections.Generic;
using Wordki.Models;

namespace Wordki.Helpers {
  public class GroupJoiner {
    List<Group> Groups { get; set; }

    public GroupJoiner(List<Group> pGroups) {
      Groups = pGroups;
    }

    public void Connect() {
      if (Groups == null || Groups.Count < 2) {
        Logger.LogError("Blad polaczenia grup - nie ma co laczyc");
        return;
      }
      Group lMainGroup = Groups[0];
      for (int i = 1; i < Groups.Count; ++i) {
        Group groupToJoin = Groups[i];
        for(int j=groupToJoin.WordsList.Count-1; j>=0; --j){
          Word wordToTransfer = groupToJoin.WordsList[j];
          wordToTransfer.GroupId = lMainGroup.Id;
          lMainGroup.WordsList.Add(wordToTransfer);
          groupToJoin.WordsList.Remove(wordToTransfer);
        }
        Groups[i].State = -1;
      }
    }
  }
}
