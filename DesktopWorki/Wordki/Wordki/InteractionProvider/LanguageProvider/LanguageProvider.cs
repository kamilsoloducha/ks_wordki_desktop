using System.Collections.Generic;
using Repository.Models.Language;
using Wordki.ViewModels.Dialogs;
using Wordki.Views.Dialogs;

namespace Wordki.InteractionProvider
{
    public class LanguageProvider : ILanguageProvider
    {
        public LanguageType SelectedType { get { return viewModel.SelectedItem; } }
        public IEnumerable<LanguageType> Items { get; set; }

        protected LanguageListDialogViewModel viewModel { get; set; }
        public LanguageProvider()
        {
            viewModel = new LanguageListDialogViewModel();
            viewModel.PositiveLabal = "Wybiuerz";
        }

        public virtual void Interact()
        {
            viewModel.Items = Items;
            viewModel.InitViewModel();
            LanguageListDialog dialog = new LanguageListDialog();
            dialog.ViewModel = viewModel;
            IDialogOrganizer dialogOrganizer = DialogOrganizerSingleton.Instance;
            dialogOrganizer.ShowDialog(dialog);
        }
    }
}
