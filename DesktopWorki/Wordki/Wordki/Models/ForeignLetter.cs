using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wordki.Models {
  public class ForeignLetter : INotifyPropertyChanged {

    private string _foreignKey;
    public string ForeignKey {
      get { return _foreignKey; }
      set {
        if (_foreignKey == value) return;
        _foreignKey = value;
        OnPropertyChanged();
      }
    }

    private char _keyboardKey;
    public char KeyboardKey {
      get { return _keyboardKey; }
      set {
        if (_keyboardKey == value) return;
        _keyboardKey = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string name = "") {
      if (PropertyChanged != null) {
        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
      }
    }
  }
}
