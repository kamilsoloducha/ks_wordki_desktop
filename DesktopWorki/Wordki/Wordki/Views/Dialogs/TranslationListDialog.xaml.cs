using System.Windows;
using Wordki.ViewModels.Dialogs;

namespace Wordki.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for TranslationListDialog.xaml
    /// </summary>
    public partial class TranslationListDialog : Window
    {
        private TranslationListDialogViewModel _viewModel;

        public TranslationListDialogViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                ViewModel.ClosingRequest += (s, e) => Close();
                DataContext = ViewModel;
            }
        }


        public TranslationListDialog()
        {
            InitializeComponent();
            Owner = App.Current.MainWindow;
            Width = Owner.ActualWidth;
            Height = Owner.ActualWidth / 3;
        }

        private void SearchDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            Activate();
        }
    }
}
