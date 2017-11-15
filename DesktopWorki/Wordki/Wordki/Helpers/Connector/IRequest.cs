using System.Collections.Generic;

namespace Wordki.Helpers.Connector
{
    public interface IRequest
    {

        string Url { get; set; }

        string Method { get; set; }

        Dictionary<string, string> Headers { get; }

        string Message { get; set; }

    }
}
