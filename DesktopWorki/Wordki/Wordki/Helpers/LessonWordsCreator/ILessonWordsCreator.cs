using Repository.Models;
using System.Collections.Generic;

namespace Wordki.Helpers
{
    public interface ILessonWordsCreator
    {
        int Count { get; set; }
        bool AllWords { get; set; }
        IList<IGroup> Groups { get; set; }
        IEnumerable<IWord> GetWords();

    }
}
