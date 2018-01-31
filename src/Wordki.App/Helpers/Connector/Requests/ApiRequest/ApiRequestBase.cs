using System.Collections.Generic;
using Wordki.Models.AppSettings;
using WordkiModel;

namespace Wordki.Helpers.Connector.Requests
{
    public abstract class ApiRequestBase : IRequest
    {
        protected abstract string Path { get; }
        public Dictionary<string, string> Headers { get; }
        public string Url
        {
            get
            {
                return $"{AppSettingsSingleton.Instance.ApiHost}/{Path}";
            }
        }
        public virtual string Method { get; set; }
        public virtual string Message { get; set; }

        public ApiRequestBase(IUser user)
        {
            Headers = new Dictionary<string, string>();
        }
    }
}
