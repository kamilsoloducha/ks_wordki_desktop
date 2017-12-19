using WordkiModel;
using System.Collections.Generic;
using System.Threading;
using Wordki.Models;

namespace Wordki.Helpers.GroupSplitter
{
    public abstract class GroupSplitterBase
    {
        public abstract IEnumerable<IGroup> Split(IGroup group, int factor);

        protected IGroup CreateGroup(IGroup group, int counter)
        {
            Group lNewGroup = new Group();
            lNewGroup.Name = group.Name + " " + new string('*', counter);
            lNewGroup.Language1 = group.Language1;
            lNewGroup.Language2 = group.Language2;
            Thread.Sleep(1);
            return lNewGroup;
        }

        protected void TransferWord(IWord word, IGroup newGroup)
        {
            word.Group.Words.Remove(word);
            word.Group = newGroup;
            newGroup.Words.Add(word);
        }
    }
}
