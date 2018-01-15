using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class GetWordsRequest : ApiRequestBase
    {
        protected override string Path { get { return "/Words/Get/"; } }

        public GetWordsRequest(IUser user) : base(user)
        {
            Method = "GET";
        }
    }
}
