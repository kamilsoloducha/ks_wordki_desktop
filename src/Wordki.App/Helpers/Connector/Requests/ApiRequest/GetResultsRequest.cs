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
            path = $"Results/{user.DownloadTime}/{user.ApiKey}";
        }
    }
}
