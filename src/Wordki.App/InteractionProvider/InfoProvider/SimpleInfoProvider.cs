using Wordki.ViewModels.Dialogs;
using Wordki.Views.Dialogs;

namespace Wordki.InteractionProvider
{
    public class SimpleInfoProvider : InteractionProviderBase, IInfoProvider
    {

        public DialogViewModelBase ViewModel { get; set; }

        protected override void DispatcherWork()
        {
            IDialogOrganizer dialogOrganizer = DialogOrganizerSingleton.Instance;
            dialogOrganizer.ShowDialog(new InfoDialog()
            {
                ViewModel = ViewModel
            });
        }
    }
}
