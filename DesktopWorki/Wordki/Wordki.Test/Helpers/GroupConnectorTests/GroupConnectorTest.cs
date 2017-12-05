using NUnit.Framework;
using Repository.Models;
using System.Collections.Generic;
using Wordki.Helpers.GroupConnector;

namespace Wordki.Test.Helpers.GroupConnectorTests
{
    [TestFixture]
    public class GroupConnectorTest
    {

        static int groupCount = 5;
        static int resultCount = 10;
        static int wordCount = 10;

        IList<IGroup> groups;
        IGroupConnector connector;
        static GroupConnectorTest()
        {
            Utility.GroupCount = groupCount;
            Utility.ResultCount = resultCount;
            Utility.WordCount = wordCount;
        }

        [SetUp]
        public void SetUp()
        {
            groups = Utility.GetGroups();
            connector = new GroupConnector();
        }

        [Test]
        public void Check_language_type_before_connection_groups_test()
        {
            groups[0].Language1 = Repository.Models.Language.LanguageType.Germany;
            Assert.IsNull(connector.DestinationGroup);
            Assert.False(connector.Connect(groups));
        }

        [Test]
        public void Check_groups_count_before_connection_test()
        {
            groups = Utility.GetGroups(1);
            Assert.IsNull(connector.DestinationGroup);
            Assert.False(connector.Connect(groups));
        }

        [Test]
        public void Check_words_count_after_connection_test()
        {
            connector.Connect(groups);
            Assert.AreEqual(groupCount * wordCount, groups[0].Words.Count);
        }

        [Test]
        public void Check_words_group_after_connection_test()
        {
            connector.Connect(groups);
            foreach (IWord word in groups[0].Words)
            {
                Assert.AreSame(groups[0], word.Group);
            }
        }

        [Test]
        public void Check_words_count_another_group_after_connection_test()
        {
            connector.Connect(groups);
            for (int i = 1; i < groups.Count; i++)
            {
                Assert.AreEqual(0, groups[i].Words.Count);
            }
        }

        [Test]
        public void Check_results_count_after_connection_test()
        {
            connector.Connect(groups);
            Assert.AreEqual(resultCount, groups[0].Results.Count);
        }

        [Test]
        public void Check_results_group_after_connection_test()
        {
            connector.Connect(groups);
            foreach (IResult result in groups[0].Results)
            {
                Assert.AreSame(groups[0], result.Group);
            }
        }

        [Test]
        public void Check_results_count_another_group_after_connection_test()
        {
            connector.Connect(groups);
            for (int i = 1; i < groups.Count; i++)
            {
                Assert.AreEqual(0, groups[i].Results.Count);
            }
        }

        [Test]
        public void Check_destination_group_after_connection_test()
        {
            connector.Connect(groups);
            Assert.AreSame(groups[0], connector.DestinationGroup);
        }

    }
}
