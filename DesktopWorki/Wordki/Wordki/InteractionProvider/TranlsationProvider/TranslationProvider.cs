using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;
using Repository.Models.Enums;
using System.Windows;
using System.ComponentModel;
using Wordki.ViewModels.Dialogs;
using Wordki.Views.Dialogs;
using Wordki.Helpers.TranslationPusher;

namespace Wordki.InteractionProvider
{
    public class TranslationProvider : ITranslationProvider
    {
        public TranslationDirection TranslationDirection { get; set; }
        public IWord Word { get; set; }
        public IEnumerable<string> Items { get; set; }

        public void Interact()
        {
            TranslationListDialogViewModel viewModel = new TranslationListDialogViewModel(Items);
            TranslationListDialog dialog = new TranslationListDialog()
            {
                ViewModel = viewModel,
            };
            if (Application.Current.Dispatcher.CheckAccess())
            {
                dialog.ShowDialog();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => dialog.ShowDialog());
            }
            if (viewModel.Canceled)
            {
                return;
            }
            IEnumerable<string> selectedItems = viewModel.Items.Where(x => x.Approved).Select(x => x.Translation);
            ITranslationPusher translationPusher = new TranslationPusher()
            {
                TranslationDirection = TranslationDirection,
                TranslationsList = selectedItems,
            };
            translationPusher.SetTranslation(Word);
        }
    }
}
