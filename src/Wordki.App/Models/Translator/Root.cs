using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wordki.Models.Translator
{
    public class Root
    {

        [JsonProperty(PropertyName = "result")]
        public string Result { get; set; }

        [JsonProperty(PropertyName = "tuc")]
        public List<Package> Packages { get; set; }

        [JsonProperty(PropertyName = "pharse")]
        public string Phrase { get; set; }
    }
}
