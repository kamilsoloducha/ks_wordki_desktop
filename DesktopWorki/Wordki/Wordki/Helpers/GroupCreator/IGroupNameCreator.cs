using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.GroupCreator
{
    public interface IGroupNameCreator
    {
        string CreateName(string input);
    }
}
