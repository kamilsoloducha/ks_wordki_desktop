using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.Connector
{
    public class TranslationParser : IParser<TranslationResponse>
    {
        public TranslationResponse Parse(string message)
        {
            TranslationResponse response = new TranslationResponse();
            response.TranslationWord = JsonConvert.DeserializeObject<Models.Translator.Root>(message);
            return response;
        }
    }
}
