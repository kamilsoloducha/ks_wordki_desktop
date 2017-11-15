using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;

namespace Wordki.Helpers.WordFinder
{
    public interface IWordFinder
    {

        IEnumerable<Word> FindWords();

    }
}
