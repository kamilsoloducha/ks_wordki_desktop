using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wordki.Models {
  public class GroupItem : INotifyPropertyChanged {

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(name));
      }
    }

    #endregion

    public Group Group { get; set; }

    private int _color;
    public int Color {
      get { return _color; }
      set {
        if (_color == value) return;
        _color = value;
        OnPropertyChanged();
      }
    }

    private int _nextRepeat;
    public int NextRepeat {
      get { return _nextRepeat; }
      set {
        if (_nextRepeat == value) return;
        _nextRepeat = value;
        OnPropertyChanged();
      }
    }


    public GroupItem(Group group) {
      Group = group;
    }
  }
}
