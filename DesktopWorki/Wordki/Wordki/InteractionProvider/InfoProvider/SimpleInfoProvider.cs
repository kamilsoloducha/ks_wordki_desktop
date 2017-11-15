using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.ViewModels.Dialogs;
using Wordki.Views.Dialogs;

namespace Wordki.InteractionProvider
{
    public class SimpleInfoProvider : InteractionProviderBase, IInfoProvider
    {

        public DialogViewModelBase ViewModel { get; set; }

        protected override void DispatcherWork()
        {
            DialogBase dialog = new InfoDialog()
            {
                ViewModel = ViewModel,
            };
            dialog.ShowDialog();
        }
    }
}
