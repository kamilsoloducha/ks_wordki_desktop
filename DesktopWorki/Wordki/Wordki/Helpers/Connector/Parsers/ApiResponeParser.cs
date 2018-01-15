using Newtonsoft.Json;

namespace Wordki.Helpers.Connector
{
    public class ApiResponeParser<T> : IParser<ApiResponse<T>>
    {
        public ApiResponse<T> Parse(string message)
        {
            ApiResponse<T> response = JsonConvert.DeserializeObject<ApiResponse<T>>(message);
            return response;
        }
    }
}
