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
            YesNoDialog dialog = new YesNoDialog();
            dialog.ViewModel = ViewModel;
            dialog.ShowDialog();
        }

    }
}
