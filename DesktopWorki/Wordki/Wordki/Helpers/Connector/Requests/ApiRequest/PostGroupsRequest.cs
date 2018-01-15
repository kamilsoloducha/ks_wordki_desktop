using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class PostGroupsRequest : ApiRequestBase
    {
        protected override string Path { get { return "Groups/Post/"; } }

        public PostGroupsRequest(IUser user) : base(user)
        {
            Method = "POST";
        }
    }
}
