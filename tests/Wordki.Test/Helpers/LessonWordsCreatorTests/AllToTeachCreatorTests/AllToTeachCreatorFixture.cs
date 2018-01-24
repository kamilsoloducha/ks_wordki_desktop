using NUnit.Framework;
using Oazachaosu.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Wordki.Helpers;
using Wordki.Models.LessonScheduler;
using WordkiModel;


namespace Wordki.Test.Helpers.LessonWordsCreatorTests.AllToTeachCreatorTests
{
    [TestFixture]
    public class AllToTeachCreatorFixture
    {

        [Test]
        public void Get_words_test()
        {
            ILessonWordsCreator creator = new AllToTeachCreator()
            {
                Scheduler = PrepareScheduler(),
            };
            creator.AllWords = true;
            creator.Groups = PrepareGroups();
            IList<IWord> result = creator.GetWords().ToList();
            Assert.AreEqual(2 * Utility.WordCount, result.Count);
        }

        private IList<IGroup> PrepareGroups()
        {
            List<IGroup> groups = new List<IGroup>();
            Utility.ResultCount = 0;
            Utility.WordCount = 10;

            IGroup group = Utility.GetGroup();
            IResult result = Utility.GetResult();
            result.DateTime = DateTime.Now.AddMonths(-1);
            group.AddResult(result);
            groups.Add(group);

            group = Utility.GetGroup();
            groups.Add(group);

            group = Utility.GetGroup();
            result = Utility.GetResult();
            result.DateTime = DateTime.Now.AddDays(-5);
            group.AddResult(result);
            result = Utility.GetResult();
            result.DateTime = DateTime.Now.AddDays(-4);
            group.AddResult(result);
            result = Utility.GetResult();
            result.DateTime = DateTime.Now.AddDays(-3);
            group.AddResult(result);
            result = Utility.GetResult();
            result.DateTime = DateTime.Now.AddDays(-2);
            group.AddResult(result);
            result = Utility.GetResult();
            result.DateTime = DateTime.Now.AddDays(-1);
            group.AddResult(result);
            groups.Add(group);

            return groups;
        }

        private ILessonScheduler PrepareScheduler()
        {
            return new NewLessonScheduler()
            {
                Initializer = new LessonSchedulerInitializer2(new List<int>() { 1, 1, 2, 4, 7 })
                {
                    TranslationDirection = TranslationDirection.FromFirst,
                },
            };
        }

    }
}
