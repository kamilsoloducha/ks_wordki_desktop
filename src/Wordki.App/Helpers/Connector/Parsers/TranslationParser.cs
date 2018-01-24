using Newtonsoft.Json;

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
