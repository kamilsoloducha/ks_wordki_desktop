
using Newtonsoft.Json;

namespace WordkiModel.DTO
{
    public class UserDTO
    {
        [JsonProperty("Id")]
        public long Id { get; set; }
        [JsonProperty("ApiKey")]
        public string ApiKey { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
