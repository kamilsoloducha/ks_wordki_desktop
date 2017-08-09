using System;
using System.ComponentModel;
using System.Windows;
using Wordki.Helpers;

namespace Wordki.Views.Dialogs {
  /// <summary>
  /// Interaction logic for InfoDialog.xaml
  /// </summary>
  public partial class InfoDialog : INotifyPropertyChanged {
    private string _dialogTitle;
    private string _message;
    private string _buttonLabel;

    public string DialogTitle {
      get { return _dialogTitle; }
      set {
        if (_dialogTitle != value) {
          _dialogTitle = value;
          OnPropertyChanged("DialogTitle");
        }
      }
    }
    public string Message {
      get { return _message; }
      set {
        if (_message != value) {
          _message = value;
          OnPropertyChanged("Messsage");
        }
      }
    }
    public string ButtonLabel {
      get { return _buttonLabel; }
      set {
        if (_buttonLabel != value) {
          _buttonLabel = value;
          OnPropertyChanged("ButtonLabel");
        }
      }
    }
    public BuilderCommand ButtonCommand { get; set; }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName) {
      PropertyChangedEventHandler handler = PropertyChanged;

      if (handler != null) {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }
    #endregion

    public InfoDialog() {
      InitializeComponent();
      InitDefaultValue();
      DataContext = this;
      Owner = Application.Current.MainWindow;
      Width = Owner.ActualWidth;
    }

    public void InitDefaultValue() {
      DialogTitle = "Tytul";
      Message = "Wiadomosc";
      ButtonLabel = "Napis";
    }

    public static void ShowInfoDialog(string pTitle, string pMessage, string pButtonLabel = "Ok", Action pAction = null) {
      InfoDialog lInfoDialog = new InfoDialog();
      lInfoDialog.DialogTitle = pTitle;
      lInfoDialog.Message = pMessage;
      lInfoDialog.ButtonLabel = pButtonLabel;
      lInfoDialog.ButtonCommand = new BuilderCommand(delegate {
        if (pAction != null) {
          pAction();
        }
        if (lInfoDialog.IsVisible) {
          lInfoDialog.Close();
        }
      });
      lInfoDialog.ShowDialog();
    }
  }
}
