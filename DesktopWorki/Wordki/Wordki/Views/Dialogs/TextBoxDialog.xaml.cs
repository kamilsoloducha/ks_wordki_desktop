using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Wordki.Helpers;

namespace Wordki.Views.Dialogs {
  /// <summary>
  /// Interaction logic for TextBoxDialog.xaml
  /// </summary>
  public partial class TextBoxDialog : Window, INotifyPropertyChanged {

    public BuilderCommand OkCommand { get; set; }
    public BuilderCommand CancelCommand { get; set; }

    private string _text;
    public string Text {
      get { return _text; }
      set {
        if (_text == value) return;
        _text = value;
        OnPropertyChanged();
      }
    }

    private string _message;
    public string Message {
      get { return _message; }
      set {
        if (_message == value) return;
        _message = value;
        OnPropertyChanged();
      }
    }

    public TextBoxDialog() {
      InitializeComponent();
      DataContext = this;
      Owner = Application.Current.MainWindow;
      Width = Owner.ActualWidth;
    }

    private void TextBoxDialog_OnLoaded(object sender, RoutedEventArgs e) {
      Activate();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string pPropertyName = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
      }
    }

    private void OnClickOkClick(object sender, RoutedEventArgs e) {
      if (OkCommand!= null)
        OkCommand.Execute(Text);
      Close();
    }

    private void OnClickCancelClick(object sender, RoutedEventArgs e) {
      if (CancelCommand != null)
        CancelCommand.Execute(null);
      Close();
    }
  }
}
