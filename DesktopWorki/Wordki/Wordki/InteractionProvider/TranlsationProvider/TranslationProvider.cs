﻿using System.Collections.Generic;
using System.Linq;
using Repository.Models;
using Repository.Models.Enums;
using System.Windows;
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
            TranslationListDialogViewModel viewModel = new TranslationListDialogViewModel(Items);
            TranslationListDialog dialog = new TranslationListDialog()
            {
                ViewModel = viewModel,
            };
            dialog.ShowDialog();
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
