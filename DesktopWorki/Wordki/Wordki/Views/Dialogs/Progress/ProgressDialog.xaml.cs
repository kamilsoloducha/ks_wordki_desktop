using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Util.Threads;
using Wordki.ViewModels.Dialogs.Progress;

namespace Wordki.Views.Dialogs.Progress
{
    /// <summary>
    /// Interaction logic for ProcessDialog.xaml
    /// </summary>
    public partial class ProgressDialog : Window, IProgressDialog
    {
        private ProgressDialogViewModel _viewModel;

        public ProgressDialogViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if(_viewModel == value)
                {
                    return;
                }
                _viewModel = value;
                ViewModel.ClosingRequest += (s, e) => Close();
                DataContext = ViewModel;
            }
        }

        public Action OnShow { get; set; }
        public Action OnClose { get; set; }

        public ProgressDialog()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            Width = Owner.ActualWidth;
        }

        public new void Show()
        {
            ShowDialog();
        }

        public new void Close()
        {
            Console.WriteLine("Close");
            if (OnClose != null)
            {
                OnClose();
            }
            base.Close();
        }

        private void ProcessDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            Activate();
        }
    }
}
