using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Wordki.Helpers;

namespace Wordki.Views.Dialogs {
  public partial class NumberChooserDialog : Window, INotifyPropertyChanged {

    private string _button1Label;
    private string _button2Label;
    private string _textBoxContent;
    private string _message;

    public string Button1Label {
      get { return _button1Label; }
      set {
        if (_button1Label != value) {
          _button1Label = value;
          OnPropertyChanged("Button1Label");
        }
      }
    }
    public string Button2Label {
      get { return _button2Label; }
      set {
        if (_button2Label != value) {
          _button2Label = value;
          OnPropertyChanged("Button2Label");
        }
      }
    }
    public string TextBoxContent {
      get { return _textBoxContent; }
      set {
        if (_textBoxContent != value) {
          _textBoxContent = value;
          OnPropertyChanged("TextBoxContent");
        }
      }
    }
    public string Message {
      get { return _message; }
      set {
        if (_message != value) {
          _message = value;
          OnPropertyChanged("Message");
        }
      }
    }
    public BuilderCommand Button1Command { get; set; }
    public BuilderCommand Button2Command { get; set; }
    public BuilderCommand LessCommand { get; set; }
    public BuilderCommand MoreCommand { get; set; }

    public NumberChooserDialog() {
      InitializeComponent();
      LessCommand = new BuilderCommand(Less);
      MoreCommand = new BuilderCommand(More);
      DataContext = this;
      Owner = App.Current.MainWindow;
      Width = Owner.ActualWidth;
    }

    private void More(object obj) {
      if (TextBoxContent == null) {
        TextBoxContent = "0";
      } else {
        int lNumber = Int32.Parse(TextBoxContent);
        lNumber++;
        TextBoxContent = lNumber.ToString();
      }
    }

    private void Less(object obj) {
      if (TextBoxContent == null) {
        TextBoxContent = "0";
      } else {
        int lNumber = Int32.Parse(TextBoxContent);
        if (lNumber == 0) return;
        lNumber--;
        TextBoxContent = lNumber.ToString();
      }
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string pPropertyname) {
      PropertyChangedEventHandler lHandler = PropertyChanged;
      if (lHandler != null) {
        lHandler(this, new PropertyChangedEventArgs(pPropertyname));
      }
    }

    #endregion

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
      if (!char.IsDigit(e.Text, e.Text.Length - 1)) {
        e.Handled = true;
      }
    }
  }
}
