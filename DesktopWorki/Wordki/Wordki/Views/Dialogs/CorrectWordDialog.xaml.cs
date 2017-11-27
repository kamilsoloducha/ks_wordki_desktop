using Repository.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Util;

namespace Wordki.Views.Dialogs {
  /// <summary>
  /// Interaction logic for FourTextBoxDialog.xaml
  /// </summary>
  public partial class CorrectWordDialog : Window, INotifyPropertyChanged {
    private string _textBox1;
    private string _textBox2;
    private string _textBox3;
    private string _textBox4;

    #region Properties
    public string Language1 {
      get { return _textBox1; }
      set {
        if (_textBox1 != value) {
          _textBox1 = value;
          OnPropertyChanged();
        }
      }
    }
    public string Language2 {
      get { return _textBox2; }
      set {
        if (_textBox2 != value) {
          _textBox2 = value;
          OnPropertyChanged();
        }
      }
    }
    public string Language1Comment {
      get { return _textBox3; }
      set {
        if (_textBox3 != value) {
          _textBox3 = value;
          OnPropertyChanged();
        }
      }
    }
    public string Language2Comment {
      get { return _textBox4; }
      set {
        if (_textBox4 != value) {
          _textBox4 = value;
          OnPropertyChanged();
        }
      }
    }
    public ICommand CorrectCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    private IWord Word { get; set; }
    #endregion

    public CorrectWordDialog(IWord pWord) {
      InitializeComponent();
      DataContext = this;
      Owner = Application.Current.MainWindow;
      Width = Owner.ActualWidth;
      Word = pWord;
      LoadDefaultValue();
    }

    private void LoadDefaultValue() {
      Language1 = Word.Language1;
      Language2 = Word.Language2;
      Language1Comment = Word.Language1Comment;
      Language2Comment = Word.Language2Comment;
      CorrectCommand = new BuilderCommand(() =>{
        Word.Language1 = Language1;
        Word.Language2 = Language2;
        Word.Language1Comment = Language1Comment;
        Word.Language2Comment = Language2Comment;
        DialogResult = false;
        Close();
      });
      DeleteCommand = new BuilderCommand(() => {
        DialogResult = true;
        Close();
      });
      CancelCommand = new BuilderCommand(() => {
        DialogResult = null;
        Close();
      });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string pPropertyName = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
      }
    }

    private void CorrectWordDialog_OnLoaded(object sender, RoutedEventArgs e) {
      Activate();
    }
  }
}
