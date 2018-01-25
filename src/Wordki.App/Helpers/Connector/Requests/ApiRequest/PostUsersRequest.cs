using Newtonsoft.Json;
using Oazachaosu.Core.Common;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class PostUsersRequest : ApiRequestBase
    {
        protected override string Path { get => "Users"; }

        public PostUsersRequest(IUser user) : base(user)
        {
            Method = "POST";
            Message = JsonConvert.SerializeObject(AutoMapper.AutoMapperConfig.Instance.Map<IUser, UserDTO>(user));
        }
    }
}
