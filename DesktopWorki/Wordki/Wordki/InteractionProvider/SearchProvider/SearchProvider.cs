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
    public class SearchProvider : InteractionProviderBase, ISearchProvider
    {
        protected override void DispatcherWork()
        {
            SearchDialogViewModel viewModel = new SearchDialogViewModel();
            viewModel.InitViewModel();
            IDialogOrganizer dialogOrganizer = DialogOrganizerSingleton.Instance;
            dialogOrganizer.ShowDialog(new SearchDialog()
            {
                ViewModel = viewModel
            });
        }
    }
}
