using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class GetUserRequest : ApiRequestBase
    {
        private string path;
        protected override string Path { get { return path; } }

        public GetUserRequest(IUser user) : base(user)
        {
            Method = "GET";
            path = $"Users/{user.Name}/{user.Password}";
        }
    }
}
