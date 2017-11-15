using Repository.Models;

namespace Wordki.Models {
  public class GroupItem : ModelBase<GroupItem> {

    public IGroup Group { get; set; }

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

    public GroupItem(IGroup group) {
      Group = group;
    }
  }
}
