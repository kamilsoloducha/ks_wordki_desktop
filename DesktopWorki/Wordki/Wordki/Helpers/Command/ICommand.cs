using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.Command
{
    public interface ICommand
    {
        bool Execute();
    }
}
