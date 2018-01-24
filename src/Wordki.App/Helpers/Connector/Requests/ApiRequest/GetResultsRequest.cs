using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class GetResultsRequest : ApiRequestBase
    {
        protected override string Path { get { return "Results/"; } }

        public GetResultsRequest(IUser user) : base(user)
        {
            Method = "GET";
        }
    }
}
