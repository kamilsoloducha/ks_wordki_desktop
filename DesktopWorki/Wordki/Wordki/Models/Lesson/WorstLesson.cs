using System.Collections.Generic;
using System.Linq;
using Repository.Models;

namespace Wordki.Models.Lesson
{
    public class WorstLesson : TypingLesson
    {
        private static int MaxCount = 30;

        public WorstLesson(IEnumerable<IGroup> groups) : base(GetWordsToLearn(groups))
        {
        }

        private static IEnumerable<IWord> GetWordsToLearn(IEnumerable<IGroup> groups)
        {
            return groups.SelectMany(x => x.Words).OrderBy(x => x.Drawer).Take(MaxCount);
        }
    }
}
