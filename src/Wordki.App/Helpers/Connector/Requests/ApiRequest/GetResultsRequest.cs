using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class GetResultsRequest : ApiRequestBase
    {
        private string path;
        protected override string Path { get { return path; } }

        public GetResultsRequest(IUser user) : base(user)
        {
            Method = "GET";
            path = $"Results/1990-01-01/{user.ApiKey}";
        }
    }
}
