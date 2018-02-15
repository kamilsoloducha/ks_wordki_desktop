using WordkiModel;
using WordkiModel.Extensions;

namespace Wordki.Helpers.Connector.Requests
{
    public class GetWordsRequest : ApiRequestBase
    {
        private string path;
        protected override string Path { get { return path; } }

        public GetWordsRequest(IUser user) : base(user)
        {
            Method = "GET";
            path = $"Words/{user.GetFormatedDateTime()}/{user.ApiKey}";
        }
    }
}
