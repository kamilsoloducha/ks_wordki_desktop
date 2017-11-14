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
    public class SearchProvider : ISearchProvider
    {
        public void Interact()
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                DispacherWork();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(DispacherWork);
            }
        }

        private void DispacherWork()
        {
            SearchDialog dialog = new SearchDialog();
            SearchDialogViewModel viewModel = new SearchDialogViewModel();
            viewModel.InitViewModel();
            dialog.ViewModel = viewModel;
            dialog.ShowDialog();
        }
    }
}
