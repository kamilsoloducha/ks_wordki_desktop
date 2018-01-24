using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public class PostUserRequest : ApiRequestBase
    {
        protected override string Path { get { return "Users/"; } }

        public PostUserRequest(IUser user) : base(user)
        {
            Method = "POST";
        }
    }
}
