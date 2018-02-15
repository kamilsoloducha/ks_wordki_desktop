using WordkiModel;
using WordkiModel.Extensions;

namespace Wordki.Helpers.Connector.Requests
{
    public class GetGroupsRequest : ApiRequestBase
    {

        private string path;
        protected override string Path { get { return path; } }

        public GetGroupsRequest(IUser user) : base(user)
        {
            Method = "GET";
            path = $"Groups/{user.GetFormatedDateTime()}/{user.ApiKey}";
        }
    }
}
