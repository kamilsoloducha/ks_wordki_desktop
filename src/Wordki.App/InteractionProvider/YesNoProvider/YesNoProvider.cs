using Wordki.ViewModels.Dialogs;
using Wordki.Views.Dialogs;

namespace Wordki.InteractionProvider
{
    public class YesNoProvider : InteractionProviderBase, IYesNoProvider
    {
        public DialogViewModelBase ViewModel { get; set; }

        public YesNoProvider()
        {
        }

        protected override void DispatcherWork()
        {
            YesNoDialog dialog = new YesNoDialog();
            dialog.ViewModel = ViewModel;
            IDialogOrganizer organizer = DialogOrganizerSingleton.Instance;
            organizer.ShowDialog(dialog);

        }
    }
}
