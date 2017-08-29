using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models.Connector;

namespace Wordki.Helpers.Command
{
    public class CommandApiRequest : ICommand
    {
        public Action<ApiResponse> OnCompleteCommand { get; set; }
        private ApiRequest Request { get; set; }

        public CommandApiRequest(ApiRequest pRequest)
        {
            Request = pRequest;
        }

        public bool Execute()
        {
            Request.PrepareMessage();
            ApiConnector lConnector = new ApiConnector();
            ApiResponse lResponse = lConnector.SentRequest(Request);
            if (OnCompleteCommand != null && lResponse != null)
            {
                OnCompleteCommand(lResponse);
            }
            return lResponse != null && !lResponse.IsError;
        }
    }
}
