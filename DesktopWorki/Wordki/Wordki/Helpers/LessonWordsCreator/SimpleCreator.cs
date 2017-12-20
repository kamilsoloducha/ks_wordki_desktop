using WordkiModel;
using System.Collections.Generic;
using System.Linq;

namespace Wordki.Helpers
{
    public class SimpleCreator : ILessonWordsCreator
    {
        public int Count { get; set; }
        public bool AllWords { get; set; }
        public IList<IGroup> Groups { get; set; }
        public IEnumerable<IWord> GetWords()
        {
            foreach(IGroup group in Groups)
            {
                foreach(IWord word in group.Words.Where(x => x.Visible || AllWords))
                {
                    yield return word;
                }
            }
        }

    }
}
