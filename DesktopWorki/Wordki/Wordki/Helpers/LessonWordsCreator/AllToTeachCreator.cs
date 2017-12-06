using System.Collections.Generic;
using System.Linq;
using Repository.Models;
using Wordki.Models.LessonScheduler;

namespace Wordki.Helpers
{
    public class AllToTeachCreator : ILessonWordsCreator
    {

        public ILessonScheduler Scheduler { get; set; }
        public IEnumerable<IGroup> Groups { get; set; }
        public bool AllWords { get; set; }

        public IEnumerable<IWord> GetWords()
        {
            foreach (IGroup group in Groups)
            {
                if (Scheduler.GetTimeToLearn(group) > 0)
                {
                    continue;
                }
                foreach (IWord word in group.Words.Where(x => x.Visible || AllWords))
                {
                    yield return word;
                }
            }
        }
    }
}
