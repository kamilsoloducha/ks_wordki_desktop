using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wordki.ViewModels.Dialogs;
using Wordki.Views.Dialogs;

namespace Wordki.InteractionProvider
{
    public class YesNoProvider : IYesNoProvider
    {
        public DialogViewModelBase ViewModel { get; set; }

        public YesNoProvider()
        {

        }

        public void Interact()
        {
            YesNoDialog dialog = new YesNoDialog();
            dialog.ViewModel = ViewModel;
            if (Application.Current.Dispatcher.CheckAccess())
            {
                dialog.ShowDialog();
            }else
            {
                Application.Current.Dispatcher.Invoke(() => dialog.ShowDialog());
            }
        }

    }
}
