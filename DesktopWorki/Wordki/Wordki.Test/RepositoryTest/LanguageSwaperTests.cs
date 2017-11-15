using NUnit.Framework;
using Repository.Helper;
using Repository.Models;
using Wordki.Models;

namespace Wordki.Test.RepositoryTests
{
    [TestFixture]
    public class LanguageSwaperTests
    {
        private ILanguageSwaper swaper;
        private Word word;
        private Utility utility = new Utility();
        
        [SetUp]
        public void SetUp()
        {
            swaper = new LanguageSwaper();
            word = utility.GetWord();
        }

        [Test]
        public void Swap_only_one_word_test()
        {
            swaper.Swap(word);
            CheckSingleWord(word);
        }

        [Test]
        public void Swap_group_check_group_test()
        {
            IGroup group = utility.GetGroup();
            swaper.Swap(group);
            Assert.AreEqual(utility.Language1, group.Language2, "Error with swap Language1Type");
            Assert.AreEqual(utility.Language2, group.Language1, "Error with swap Language2Type");
        }

        [Test]
        public void Swap_group_check_words_test()
        {
            IGroup group = utility.GetGroup();
            swaper.Swap(group);
            foreach(Word word in group.Words)
            {
                CheckSingleWord(word);
            }
        }

        [Test]
        public void Swap_group_check_results_test()
        {
            IGroup group = utility.GetGroup();
            swaper.Swap(group);
            foreach (Result result in group.Results)
            {
                Assert.AreEqual(Repository.Models.Enums.TranslationDirection.FromFirst, result.TranslationDirection);
            }
        }

        private void CheckSingleWord(Word word)
        {
            Assert.AreEqual("Language2", word.Language1, "Error in Language1 swap");
            Assert.AreEqual("Language1", word.Language2, "Error in Language2 swap");
            Assert.AreEqual("Language2Comment", word.Language1Comment, "Error in Language1Commnet swap");
            Assert.AreEqual("Language1Comment", word.Language2Comment, "Error in Language2Comment swap");
        }
    }
}
