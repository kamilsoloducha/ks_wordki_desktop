using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                ViewModel.ClosingRequest += (s, e) => Close();
                DataContext = ViewModel;
            }
        }

        public DialogBase()
        {
            Owner = App.Current.MainWindow;
            Width = Owner.ActualWidth;
        }

        protected void SearchDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            Activate();
        }
    }
}
