using System;
using Util.Threads;
using Wordki.ViewModels.Dialogs;
using Wordki.Views.Dialogs;

namespace Wordki.InteractionProvider
{
    public class ProcessProvider : InteractionProviderBase, IProgressDialog
    {

        private IDialogOrganizer organizer;
        private ProgressDialog dialog;

        public DialogViewModelBase ViewModel { get; set; }

        public ProcessProvider()
        {
            organizer = DialogOrganizerSingleton.Instance;
            dialog = new ProgressDialog();
        }

        public void Close()
        {
            organizer.HideDialog();
        }

        public void Show()
        {
            DispatcherWork();
        }

        protected override void DispatcherWork()
        {
            organizer.ShowDialog(new ProgressDialog()
            {
                ViewModel = ViewModel,
            });
        }
    }
}
