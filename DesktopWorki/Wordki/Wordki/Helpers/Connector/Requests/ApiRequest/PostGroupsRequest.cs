using Newtonsoft.Json;
using Repository.Model.DTOConverters;
using System.Collections.Generic;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class PostGroupsRequest : ApiRequestBase
    {
        protected override string Path { get { return "Groups/Post/"; } }

        public PostGroupsRequest(IUser user, IEnumerable<IGroup> groups) : base(user)
        {
            Method = "POST";
            Message = JsonConvert.SerializeObject(new { user.ApiKey, Data = GroupConverter.GetDTOsFromGroups(groups) });
        }
    }
}
