using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Wordki.Models;

namespace Wordki.Helpers.GroupSplitter
{
    public abstract class GroupSplitterBase
    {
        public abstract IEnumerable<Group> Split(Group group, int factor);

        protected Group CreateGroup(Group group, int counter)
        {
            Group lNewGroup = new Group();
            lNewGroup.Name = group.Name + " " + new string('*', counter);
            lNewGroup.Language1 = group.Language1;
            lNewGroup.Language2 = group.Language2;
            Thread.Sleep(1);
            return lNewGroup;
        }

        protected void TransferWord(Word oldWord, Group newGroup)
        {
            Word newWord = new Word
            {
                GroupId = newGroup.Id,
                Language1 = oldWord.Language1,
                Language2 = oldWord.Language2,
                Drawer = oldWord.Drawer,
                Language1Comment = oldWord.Language1Comment,
                Language2Comment = oldWord.Language2Comment,
                Visible = oldWord.Visible,
            };
            newGroup.WordsList.Add(newWord);
        }

        #region TestHelper

#if TEST

        public Group CreateGroupTest(Group group, int counter)
        {
            return CreateGroup(group, counter);
        }

        public void TransferWordTest(Word oldWord, Group newGroup)
        {
            TransferWord(oldWord, newGroup);
        }

#endif


        #endregion
    }
}
