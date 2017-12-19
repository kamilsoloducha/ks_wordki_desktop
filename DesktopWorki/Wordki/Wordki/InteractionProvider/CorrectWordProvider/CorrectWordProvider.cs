using WordkiModel;
using System;
using Wordki.ViewModels.Dialogs;
using Wordki.Views.Dialogs;

namespace Wordki.InteractionProvider
{
    public class CorrectWordProvider : InteractionProviderBase, ICorrectWordProvider
    {

        public IWord Word { get; set; }
        public Action OnCorrect { get; set; }
        public Action OnRemove { get; set; }
        public Action OnClose { get; set; }

        protected override void DispatcherWork()
        {
            CorrectWordDialog dialog = new CorrectWordDialog();
            dialog.ViewModel = new CorrectWordDialogViewModel(this);
            DialogOrganizerSingleton.Instance.ShowDialog(dialog);
        }
    }
}
