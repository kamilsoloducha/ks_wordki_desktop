using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.ResultConnector
{
    public interface IResultConnector
    {

        bool Connect(IGroup dest, IGroup src);

        void Connect(IResult dest, IResult src);

    }
}
