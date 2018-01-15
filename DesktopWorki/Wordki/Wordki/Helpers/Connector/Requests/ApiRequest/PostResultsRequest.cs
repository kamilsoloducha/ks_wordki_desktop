using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class PostResultsRequest : ApiRequestBase
    {
        protected override string Path { get { return "Results/Post/"; } }

        public PostResultsRequest(IUser user) : base(user)
        {
            Method = "POST";
        }
    }
}
