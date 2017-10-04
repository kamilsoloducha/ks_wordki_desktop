using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Wordki.Helpers;

namespace Wordki.Views.Dialogs {
  /// <summary>
  /// Interaction logic for YesNoDialog.xaml
  /// </summary>
  public partial class YesNoDialog : Window, INotifyPropertyChanged {

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string property = "") {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(property));
    }
    #endregion

    private string _dialogTitle;
    private string _message;
    private string _positiveLabel;
    private string _negativeLabel;

    #region Properties
    public string DialogTitle {
      get { return _dialogTitle; }
      set {
        if (_dialogTitle != value) {
          _dialogTitle = value;
          OnPropertyChanged();
        }
      }
    }
    public string Message {
      get { return _message; }
      set {
        if (_message != value) {
          _message = value;
          OnPropertyChanged();
        }
      }
    }
    public string PositiveLabel {
      get { return _positiveLabel; }
      set {
        if (_positiveLabel != value) {
          _positiveLabel = value;
          OnPropertyChanged();
        }
      }
    }
    public string NegativeLabel {
      get { return _negativeLabel; }
      set {
        if (_negativeLabel != value) {
          _negativeLabel = value;
          OnPropertyChanged();
        }
      }
    }
    public ICommand PositiveCommand { get; set; }
    public ICommand NegativeCommand { get; set; }

    #endregion

    public YesNoDialog() {
      InitializeComponent();
      DataContext = this;
      Owner = App.Current.MainWindow;
      Width = Owner.ActualWidth;
    }

    private void YesNoDialog_OnLoaded(object sender, RoutedEventArgs e) {
      Activate();
    }
  }
}
