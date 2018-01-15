using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class PostWordsRequest : ApiRequestBase
    {
        protected override string Path { get { return "Words/Post/"; } }

        public PostWordsRequest(IUser user): base(user)
        {
            Method = "POST";
        }
    }
}
