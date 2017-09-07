using System.Collections.Generic;
using System.Threading;
using Wordki.Models;
using Database = Wordki.Models.Database;

namespace Wordki.Helpers.GroupSplitter {
  public abstract class GroupSplitterBase {

    public int Number { get; set; }
    public Group Group { get; set; }
    private IDatabase Database { get; set; }

    protected GroupSplitterBase(IDatabase database) {
      Database = database;
    }

    public abstract IEnumerable<Group> Split();


    protected Group CreateGroup(int pCounter) {
      Group lNewGroup = new Group();
      lNewGroup.Name = Group.Name + " " + new string('*', pCounter);
      lNewGroup.Language1 = Group.Language1;
      lNewGroup.Language2 = Group.Language2;
      Thread.Sleep(1);
      return lNewGroup;
    }

    protected async void TransferWord(Word oldWord, Group newGroup) {
      Word newWord = new Word{
        GroupId = newGroup.Id,
        Language1 = oldWord.Language1,
        Language2 = oldWord.Language2,
        Drawer = oldWord.Drawer,
        Language1Comment = oldWord.Language1Comment,
        Language2Comment = oldWord.Language2Comment,
        Visible = oldWord.Visible,
      };
      await Database.DeleteWordAsync(Group, oldWord);
      await Database.AddWordAsync(newGroup, newWord);
    }
  }
}
