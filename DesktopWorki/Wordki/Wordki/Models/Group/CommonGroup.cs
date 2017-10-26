using Newtonsoft.Json;

namespace Wordki.Models {
  public class CommonGroup : Group {

    [JsonProperty(PropertyName = "words")]
    public int WordsCount { get; set; }

  }
}
