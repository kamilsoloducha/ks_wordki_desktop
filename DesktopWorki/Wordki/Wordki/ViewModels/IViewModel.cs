using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Helpers;

namespace Wordki.ViewModels
{
    public interface IViewModel
    {
        Switcher Switcher { get; set; }
        void InitViewModel();
        void Back();

        void Loaded();
        void Unloaded();
    }
}
