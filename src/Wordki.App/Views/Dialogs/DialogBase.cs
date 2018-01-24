using System.Windows;
using Wordki.ViewModels.Dialogs;

namespace Wordki.Views.Dialogs
{
    public class DialogBase : Window
    {
        private DialogViewModelBase _viewModel;

        public DialogViewModelBase ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                _viewModel.InitViewModel();
                ViewModel.ClosingRequest += (s, e) => Close();
                DataContext = ViewModel;
            }
        }

        public DialogBase()
        {
            Owner = App.Current.MainWindow;
            Width = Owner.ActualWidth;
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            Activate();
        }
    }
}
