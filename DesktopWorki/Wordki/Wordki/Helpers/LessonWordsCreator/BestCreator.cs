using WordkiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Collections;

namespace Wordki.Helpers
{
    public class BestCreator : ILessonWordsCreator
    {
        public int Count { get; set; }
        public bool AllWords { get; set; }
        public IList<IGroup> Groups { get; set; }
        public IEnumerable<IWord> GetWords()
        {
            return Groups.SelectMany(x => x.Words.Where(y => y.Drawer > 3)).ToList().Shuffle().Take(Count);
        }
    }
}
