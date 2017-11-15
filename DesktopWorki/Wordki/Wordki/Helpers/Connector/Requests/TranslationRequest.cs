using System;
using System.Collections.Generic;

namespace Wordki.Helpers.Connector.Requests
{
    public class TranslationRequest : IRequest
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Message { get; set; }


        public TranslationRequest(Models.Translator.RequestBag bag)
        {
            Url = "https://" + $"glosbe.com/gapi/translate?from={bag.From}&dest={bag.To}&format=json&phrase={bag.Word}";
            Method = "GET";
        }
    }
}
