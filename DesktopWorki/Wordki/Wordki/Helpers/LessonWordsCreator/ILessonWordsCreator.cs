using Repository.Models;
using System.Collections.Generic;

namespace Wordki.Helpers
{
    public interface ILessonWordsCreator
    {
        bool AllWords { get; set; }
        IEnumerable<IGroup> Groups { get; set; }
        IEnumerable<IWord> GetWords();

    }
}
