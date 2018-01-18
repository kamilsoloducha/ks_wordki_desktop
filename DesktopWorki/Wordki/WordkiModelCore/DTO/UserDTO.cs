
using Newtonsoft.Json;

namespace WordkiModelCore.DTO
{
    public class UserDTO
    {
        [JsonProperty("Id")]
        public long Id { get; set; }
        [JsonProperty("AK")]
        public string ApiKey { get; set; }
        [JsonProperty("N")]
        public string Name { get; set; }
        [JsonProperty("Pw")]
        public string Password { get; set; }
    }
}
