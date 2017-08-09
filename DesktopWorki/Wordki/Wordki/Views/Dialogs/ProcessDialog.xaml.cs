using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Wordki.Helpers;

namespace Wordki.Views.Dialogs {
  /// <summary>
  /// Interaction logic for ProcessDialog.xaml
  /// </summary>
  public partial class ProcessDialog : INotifyPropertyChanged {

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string property = "") {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }
    #endregion

    private string _dialogTitle;
    public string DialogTitle {
      get { return _dialogTitle; }
      set {
        if (_dialogTitle != value) {
          _dialogTitle = value;
          OnPropertyChanged();
        }
      }
    }

    private string _message;
    public string Message {
      get { return _message; }
      set {
        if (_message != value) {
          _message = value;
          OnPropertyChanged();
        }
      }
    }

    private string _cancelLabel;
    public string CancelLabel {
      get { return _cancelLabel; }
      set {
        if (_cancelLabel != value) {
          _cancelLabel = value;
          OnPropertyChanged();
        }
      }
    }
    public BuilderCommand CancelCommand { get; set; }

    public ProcessDialog() {
      InitializeComponent();
      DataContext = this;
      Owner = Application.Current.MainWindow;
      Width = Owner.ActualWidth;
    }

    private void ProcessDialog_OnLoaded(object sender, RoutedEventArgs e) {
      Activate();
    }
  }
}
