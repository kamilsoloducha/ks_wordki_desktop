using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.Connector
{
    public interface IParser<T> where T : IResponse
    {

        T Parse(string message);

    }
}
