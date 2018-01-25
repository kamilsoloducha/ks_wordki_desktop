using Newtonsoft.Json;
using Oazachaosu.Core.Common;
using System.Collections.Generic;
using Wordki.Helpers.AutoMapper;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class PostResultsRequest : ApiRequestBase
    {
        protected override string Path { get { return "Results/Post/"; } }

        public PostResultsRequest(IUser user, IEnumerable<IResult> resutls) : base(user)
        {
            Method = "POST";
            Message = JsonConvert.SerializeObject(new { user.ApiKey, Data = AutoMapperConfig.Instance.Map<IEnumerable<IResult>, IEnumerable<ResultDTO>>(resutls) });
        }
    }
}
