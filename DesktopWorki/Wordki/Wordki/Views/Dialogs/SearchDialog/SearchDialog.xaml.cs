using System.Windows;
using Wordki.ViewModels.Dialogs;

namespace Wordki.Views.Dialogs.SearchDialog
{
    /// <summary>
    /// Interaction logic for SearchDialog.xaml
    /// </summary>
    public partial class SearchDialog : Window
    {

        private SearchDialogViewModel _viewModel;

        public SearchDialogViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if(_viewModel == value)
                {
                    return;
                }
                _viewModel = value;
                _viewModel.ClosingRequest += (s, e) => Close();
                ViewModel.InitViewModel();
                DataContext = ViewModel;
            }
        }

        public SearchDialog()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            Width = Owner.Width;
        }

        private void SearchDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            Activate();
        }
    }
}
