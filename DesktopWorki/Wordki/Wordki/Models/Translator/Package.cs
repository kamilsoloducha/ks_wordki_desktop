using Newtonsoft.Json;
using System.Collections.Generic;

namespace Wordki.Models.Translator
{
    public class Package
    {
        [JsonProperty(PropertyName ="phrase")]
        public Translation Translation { get; set; }

        [JsonProperty(PropertyName = "meanings")]
        public List<Translation> Descriptions { get; set; }
    }
}
