using System;
using System.Collections.Generic;
using System.Windows.Input;
using Wordki.InteractionProvider;
using WordkiModel;

namespace Wordki.ViewModels.Dialogs
{
    public class LanguageListDialogViewModel : DialogViewModelBase
    {

        private IEnumerable<LanguageType> items;
        public IEnumerable<LanguageType> Items
        {
            get { return items; }
            set
            {
                if (items == value)
                {
                    return;
                }
                items = value;
                OnPropertyChanged();
            }
        }

        private LanguageType selectedItem;
        public LanguageType SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem == value)
                {
                    return;
                }
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        private string positiveLabel;
        public string PositiveLabal
        {
            get { return positiveLabel; }
            set
            {
                if (positiveLabel == value)
                {
                    return;
                }
                positiveLabel = value;
                OnPropertyChanged();
            }
        }


        public ICommand PositiveCommand { get; }

        public Action PositiveAction { get; set; }


        public LanguageListDialogViewModel() : base()
        {
            PositiveCommand = new Util.BuilderCommand(Positive);
        }

        private void Positive()
        {
            if (PositiveAction != null)
            {
                PositiveAction();
            }
            Close();
        }
    }
}
