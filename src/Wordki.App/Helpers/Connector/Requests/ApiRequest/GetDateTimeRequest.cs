using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class GetDateTimeRequest : ApiRequestBase
    {
        protected override string Path => "DateTime";

        public GetDateTimeRequest(IUser user) : base(user)
        {
            Method = "GET";
        }
    }
}
