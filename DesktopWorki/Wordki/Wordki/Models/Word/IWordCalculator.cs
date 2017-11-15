using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Models
{
    public interface IWordCalculator
    {

        IEnumerable<int> GetDrawerCount();

    }
}
