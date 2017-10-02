using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;

namespace Wordki.Helpers.WordConnector
{
    public interface IWordConnector
    {

        void Connect(IEnumerable<Word> words);

    }
}
