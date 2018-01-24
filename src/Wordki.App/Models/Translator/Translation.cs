using Newtonsoft.Json;

namespace Wordki.Models.Translator
{
    public class Translation
    {
        [JsonProperty(PropertyName ="text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }
    }
}
