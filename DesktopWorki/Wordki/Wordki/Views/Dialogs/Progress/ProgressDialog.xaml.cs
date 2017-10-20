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
        public System.Windows.Input.ICommand CancelCommand { get; private set; }

        public ProgressDialogViewModel ViewModel { get; set; }

        public ProgressDialog()
        {
            InitializeComponent();
            DataContext = ViewModel;
            Owner = Application.Current.MainWindow;
            Width = Owner.ActualWidth;
            CancelCommand = new Helpers.BuilderCommand(Cancel);
        }

        public new void Show()
        {
            ShowDialog();
        }

        public new void Close()
        {
            base.Close();
        }

        private void ProcessDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            Activate();
        }

        private void Cancel(object obj)
        {
            Close();
            if (ViewModel.OnCanceled != null)
            {
                ViewModel.OnCanceled();
            }
        }
    }
}
