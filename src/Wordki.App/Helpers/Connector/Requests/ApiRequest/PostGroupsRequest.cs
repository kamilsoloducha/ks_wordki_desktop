using Newtonsoft.Json;
using Oazachaosu.Core.Common;
using System.Collections.Generic;
using Wordki.Helpers.AutoMapper;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class PostGroupsRequest : ApiRequestBase
    {
        protected override string Path { get { return "Groups/"; } }

        public PostGroupsRequest(IUser user, IEnumerable<IGroup> groups) : base(user)
        {
            Method = "POST";
            Message = JsonConvert.SerializeObject(new { user.ApiKey, Data = AutoMapperConfig.Instance.Map<IEnumerable<IGroup>, IEnumerable<GroupDTO>>(groups) });
        }
    }
}
