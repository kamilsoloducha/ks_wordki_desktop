using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.Connector
{
    public class TranslationResponse : IResponse
    {

        public Models.Translator.Root TranslationWord { get; set; }

    }
}
