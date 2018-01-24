using WordkiModel;
using System.Collections.Generic;
using System.Linq;

namespace Wordki.Models
{
    public class WordCalculator : IWordCalculator
    {

        public IEnumerable<IGroup> Groups { get; set; }

        public IEnumerable<int> GetDrawerCount()
        {
            int[] drawers = new int[5];
            foreach(IWord word in Groups.SelectMany(x => x.Words))
            {
                drawers[word.Drawer]++;
            }
            return drawers;
        }
    }
}
