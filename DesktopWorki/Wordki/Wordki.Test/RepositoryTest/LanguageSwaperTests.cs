using NUnit.Framework;
using Repository.Helper;
using Repository.Models;
using Repository.Models.Language;
using Wordki.Models;

namespace Wordki.Test.RepositoryTests
{
    [TestFixture]
    public class LanguageSwaperTests
    {
        private ILanguageSwaper swaper;
        private Word word;
        
        
        [SetUp]
        public void SetUp()
        {
            swaper = new LanguageSwaper();
            word = Utility.GetWord();
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
            LanguageType type1 = LanguageType.French;
            LanguageType type2 = LanguageType.Polish;
            IGroup group = Utility.GetGroup(language1: type1, language2: type2);
            swaper.Swap(group);
            Assert.AreEqual(type1, group.Language2, "Error with swap Language1Type");
            Assert.AreEqual(type2, group.Language1, "Error with swap Language2Type");
        }

        [Test]
        public void Swap_group_check_words_test()
        {
            IGroup group = Utility.GetGroup();
            swaper.Swap(group);
            foreach(Word word in group.Words)
            {
                CheckSingleWord(word);
            }
        }

        [Test]
        public void Swap_group_check_results_test()
        {
            IGroup group = Utility.GetGroup();
            swaper.Swap(group);
            foreach (Result result in group.Results)
            {
                Assert.AreEqual(Repository.Models.Enums.TranslationDirection.FromFirst, result.TranslationDirection);
            }
        }

        private void CheckSingleWord(Word word)
        {
            Assert.AreEqual("lang2", word.Language1, "Error in Language1 swap");
            Assert.AreEqual("lang1", word.Language2, "Error in Language2 swap");
            Assert.AreEqual("lang2Comment", word.Language1Comment, "Error in Language1Commnet swap");
            Assert.AreEqual("lang1Comment", word.Language2Comment, "Error in Language2Comment swap");
        }
    }
}
