using System.Collections.Generic;

namespace Wordki.Helpers.Connector
{
    public interface IRequest
    {

        string Url { get; }

        string Method { get; }

        Dictionary<string, string> Headers { get; }

        string Message { get; }

    }
}
