using Newtonsoft.Json;
using Oazachaosu.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Helpers.AutoMapper;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class PostWordsRequest : ApiRequestBase
    {
        protected override string Path { get { return "Words/Post/"; } }

        public PostWordsRequest(IUser user, IEnumerable<IWord> words) : base(user)
        {
            Method = "POST";
            Message = JsonConvert.SerializeObject(new { user.ApiKey, Data = AutoMapperConfig.Instance.Map<IEnumerable<IWord>, IEnumerable<WordDTO>>(words) });
        }
    }
}
