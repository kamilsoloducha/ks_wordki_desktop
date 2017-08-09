using System.ComponentModel;
using System.Windows;
using Wordki.Helpers;

namespace Wordki.Views.Dialogs {
  /// <summary>
  /// Interaction logic for FourTextBoxDialog.xaml
  /// </summary>
  public partial class FourTextBoxDialog : Window, INotifyPropertyChanged {
    private string _topText;
    private string _label1;
    private string _label2;
    private string _label4;
    private string _label3;
    private string _textBox1;
    private string _textBox2;
    private string _textBox3;
    private string _textBox4;
    private string _button1Label;
    private string _button2Label;
    private string _button3Label;

    #region Properties

    public string TopText {
      get { return _topText; }
      set {
        if (_topText != value) {
          _topText = value;
          OnPropertyChanged("TopText");
        }
      }
    }
    public string Label1 {
      get { return _label1; }
      set {
        if (_label1 != value) {
          _label1 = value;
          OnPropertyChanged("Label1");
        }
      }
    }
    public string Label2 {
      get { return _label2; }
      set {
        if (_label2 != value) {
          _label2 = value;
          OnPropertyChanged("Label2");
        }
      }
    }
    public string Label3 {
      get { return _label3; }
      set {
        if (_label3 != value) {
          _label3 = value;
          OnPropertyChanged("Label3");
        }
      }
    }
    public string Label4 {
      get { return _label4; }
      set {
        if (_label4 != value) {
          _label4 = value;
          OnPropertyChanged("Label4");
        }
      }
    }
    public string TextBox1 {
      get { return _textBox1; }
      set {
        if (_textBox1 != value) {
          _textBox1 = value;
          OnPropertyChanged("TextBox1");
        }
      }
    }
    public string TextBox2 {
      get { return _textBox2; }
      set {
        if (_textBox2 != value) {
          _textBox2 = value;
          OnPropertyChanged("TextBox2");
        }
      }
    }
    public string TextBox3 {
      get { return _textBox3; }
      set {
        if (_textBox3 != value) {
          _textBox3 = value;
          OnPropertyChanged("TextBox3");
        }
      }
    }
    public string TextBox4 {
      get { return _textBox4; }
      set {
        if (_textBox4 != value) {
          _textBox4 = value;
          OnPropertyChanged("TextBox2");
        }
      }
    }
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
    public string Button3Label {
      get { return _button3Label; }
      set {
        if (_button3Label != value) {
          _button3Label = value;
          OnPropertyChanged("Button3Label");
        }
      }
    }
    public BuilderCommand Button1Command { get; set; }
    public BuilderCommand Button2Command { get; set; }
    public BuilderCommand Button3Command { get; set; }
    #endregion

    public FourTextBoxDialog(int pButtonCount) {
      InitializeComponent();
      switch (pButtonCount) {
        case 1:
          Button1.Visibility = Visibility.Collapsed;
          Button2.Visibility = Visibility.Collapsed;
          break;
        case 2:
          Button1.Visibility = Visibility.Collapsed;
          break;
      }


      DataContext = this;
      LoadDefaultValue();
    }

    private void LoadDefaultValue() {
      TopText = "Title";
      Label1 = "Label1";
      Label2 = "Label2";
      Label3 = "Label3";
      Label4 = "Label4";
      TextBox1 = "TextBox1";
      TextBox2 = "TextBox2";
      TextBox3 = "TextBox3";
      TextBox4 = "TextBox4";
      Button1Label = "ButtonLabel1";
      Button2Label = "ButtonLabel2";
      Button3Label = "ButtonLabel3";
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string pPropertyName) {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) {
        handler(this, new PropertyChangedEventArgs(pPropertyName));
      }
    }
  }
}
