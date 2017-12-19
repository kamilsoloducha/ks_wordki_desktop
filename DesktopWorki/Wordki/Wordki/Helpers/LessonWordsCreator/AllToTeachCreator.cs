using System.Collections.Generic;
using System.Linq;
using WordkiModel;
using Wordki.Models.LessonScheduler;

namespace Wordki.Helpers
{
    public class AllToTeachCreator : ILessonWordsCreator
    {
        public int Count { get; set; }

        //private ILessonScheduler lessonScheduler = new NewLessonScheduler()
        //{
        //    Initializer = new LessonSchedulerInitializer2(new List<int>() { 1, 1, 2, 4, 7 })
        //    {
        //        TranslationDirection = UserManagerSingleton.Instence.User.TranslationDirection,
        //    },
        //};
        public ILessonScheduler Scheduler
        {
            get;set;
        }

        public IList<IGroup> Groups { get; set; }
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
