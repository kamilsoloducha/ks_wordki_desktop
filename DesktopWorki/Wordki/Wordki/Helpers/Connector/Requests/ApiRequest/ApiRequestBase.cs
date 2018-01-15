using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public abstract class ApiRequestBase : IRequest
    {
        private const string TagDateTime = "dateTime";
        private const string TagApiKey = "apikey";


        protected abstract string Path { get; }
        public string Url { get; set; }
        public string Method { get; set; }

        public Dictionary<string, string> Headers { get; }

        public string Message { get; set; }

        public ApiRequestBase(IUser user)
        {
            Headers = new Dictionary<string, string>();
            Headers.Add(TagDateTime, user.DownloadTime.ToShortDateString());
            Headers.Add(TagApiKey, user.ApiKey);
            Url = $"https://localhost:44326/{Path}";
        }
    }
}
