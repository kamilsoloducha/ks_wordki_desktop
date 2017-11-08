using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.ViewModels.Dialogs;

namespace Wordki.InteractionProvider
{
    public interface IYesNoProvider
    {

        DialogViewModelBase ViewModel { get; set; }

        void Interact();
    }
}
