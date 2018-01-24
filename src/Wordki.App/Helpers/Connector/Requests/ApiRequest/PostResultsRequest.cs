using Newtonsoft.Json;
using Repository.Model.DTOConverters;
using System.Collections.Generic;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class PostResultsRequest : ApiRequestBase
    {
        protected override string Path { get { return "Results/Post/"; } }

        public PostResultsRequest(IUser user, IEnumerable<IResult> resutls) : base(user)
        {
            Method = "POST";
            Message = JsonConvert.SerializeObject(new { user.ApiKey, Data = ResultConverter.GetDTOsFromResults(resutls) });
        }
    }
}
