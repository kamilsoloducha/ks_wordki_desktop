using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Wordki.Views.Dialogs.Progress
{
    /// <summary>
    /// Interaction logic for ProcessDialog.xaml
    /// </summary>
    public partial class ProcessDialog : Window, INotifyPropertyChanged, IProgressNotification
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion

        #region Properties

        private string _dialogTitle;
        public string DialogTitle
        {
            get { return _dialogTitle; }
            set
            {
                if (_dialogTitle != value)
                {
                    _dialogTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _cancelLabel;
        public string ButtonLabel
        {
            get { return _cancelLabel; }
            set
            {
                if (_cancelLabel != value)
                {
                    _cancelLabel = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _canCanceled;
        public bool CanCanceled
        {
            get { return _canCanceled; }
            set
            {
                if(_canCanceled == value)
                {
                    return;
                }
                _canCanceled = value;
                OnPropertyChanged();
            }
        }

        public System.Action OnCanceled { get; set; }

        public System.Windows.Input.ICommand CancelCommand { get; private set; }

        #endregion

        public ProcessDialog()
        {
            InitializeComponent();
            DataContext = this;
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
            if (OnCanceled != null)
            {
                OnCanceled();
            }
        }
    }
}
