using NUnit.Framework;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Helpers;

namespace Wordki.Test.Helpers.LessonWordsCreatorTests.SimpleCreatorTests
{
    [TestFixture(3)]
    [TestFixture(1)]
    public class SimpleCreatorFromGroupsListFixture
    {
        ILessonWordsCreator creator;
        int groupCount;
        IList<IGroup> groups;

        public SimpleCreatorFromGroupsListFixture(int groupCount)
        {
            this.groupCount = groupCount;
            groups = Utility.GetGroups(groupCount);
        }

        [SetUp]
        public void SetUp()
        {
            creator = new SimpleCreator();
            creator.Count = 30;
            creator.Groups = groups;
        }

        [Test]
        public void Try_to_get_words_from_groups_if_all_words_test()
        {
            creator.AllWords = true;
            IList<IWord> list = creator.GetWords().ToList();
            Assert.AreEqual(groupCount * Utility.WordCount, list.Count);
        }


        [Test]
        public void Try_to_get_words_from_groups_if_not_all_words_test()
        {
            creator.AllWords = false;
            groups[0].Words[0].Visible = false;
            IList<IWord> list = creator.GetWords().ToList();
            Assert.AreEqual(groupCount * Utility.WordCount -1, list.Count);
        }

    }
}
