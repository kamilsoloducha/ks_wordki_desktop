using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Wordki.Models
{
    public interface ISettings
    {

        ISettings Load();
        void Save();

    }
}
