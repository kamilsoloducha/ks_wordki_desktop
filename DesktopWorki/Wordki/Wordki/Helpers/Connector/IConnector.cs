using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.Connector
{
    public interface IConnector<T> where T : IResponse
    {

        IParser<T> Parser { get; set; }

        T SendRequest(IRequest request);

    }
}
