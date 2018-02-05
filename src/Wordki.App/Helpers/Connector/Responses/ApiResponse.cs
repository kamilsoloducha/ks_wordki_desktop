namespace Wordki.Helpers.Connector
{
    public class ApiResponse<T>
    {
        public T Object { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
