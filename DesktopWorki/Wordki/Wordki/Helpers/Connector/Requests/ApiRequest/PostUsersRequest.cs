using Newtonsoft.Json;
using Repository.Model.DTOConverters;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class PostUsersRequest : ApiRequestBase
    {
        protected override string Path { get => "Users"; }

        public PostUsersRequest(IUser user) : base(user)
        {
            Method = "POST";
            Message = JsonConvert.SerializeObject(UserConverter.GetDTOFromModel(user));
        }
    }
}
