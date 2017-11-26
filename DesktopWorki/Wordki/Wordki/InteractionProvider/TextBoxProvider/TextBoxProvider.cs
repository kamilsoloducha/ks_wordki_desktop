using Wordki.ViewModels.Dialogs;
using Wordki.Views.Dialogs;

namespace Wordki.InteractionProvider
{
    public class TextBoxProvider : InteractionProviderBase
    {

        public TextBoxDialogViewModel ViewModel { get; set; }

        protected override void DispatcherWork()
        {
            TextBoxDialog dialog = new TextBoxDialog()
            {
                ViewModel = ViewModel,
            };
            DialogOrganizerSingleton.Instance.ShowDialog(dialog);
        }
    }
}
