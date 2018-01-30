using NUnit.Framework;
using Wordki.Models;
using WordkiModel;
using WordkiModel.Extensions;

namespace Wordki.Test.RepositoryTest
{
    [TestFixture]
    public class GroupsTests
    {

        IGroup group;
        IWord word;
        IResult result;

        [SetUp]
        public void SetUp()
        {
            group = new Group();
            word = new Word();
            result = new Result();
        }

        [Test]
        public void Check_references_in_word_test()
        {
            group.AddWord(word);

            Assert.AreSame(group, word.Group);
        }

        [Test]
        public void Check_references_in_result_test()
        {
            group.AddResult(result);

            Assert.AreSame(group, result.Group);
        }

    }
}
