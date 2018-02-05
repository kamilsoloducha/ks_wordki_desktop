using Newtonsoft.Json;

namespace Wordki.Helpers.Connector
{
    public class ApiResponeParser<T> : IParser<ApiResponse<T>>
    {
        public ApiResponse<T> Parse(string message)
        {
            return JsonConvert.DeserializeObject<ApiResponse<T>>(message);
        }
    }
}
