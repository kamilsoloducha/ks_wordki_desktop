using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;

namespace Wordki.Helpers.WordComparer
{
    public interface IWordComparer
    {
        bool IsEqual(Word word1, Word word2);
    }
}
