using NUnit.Framework;
using Repository.Models.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;
using Wordki.Models.LanguageSwaper;

namespace Wordki.Test.Helpers.LanguageSwaperTests
{
    [TestFixture]
    public class LanguageSwaperTest
    {

        private ILanguageSwaper swaper;



        public void Swap_nullGroup()
        {
            swaper = MakeSwaper();

            swaper.Swap(null);

            Assert.Pass("Test passed without exeption");
        }

        public void Swap_CustomGroup()
        {
            swaper = MakeSwaper();
            LanguageType lang1 = LanguageType.English;
            LanguageType lang2 = LanguageType.Polish;
            Group group = new Group { Language1 = lang1, Language2 = lang2 };
        }

        private ILanguageSwaper MakeSwaper()
        {
            return new LanguageSwaper();
        }

    }
}
