using NUnit.Framework;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;

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
