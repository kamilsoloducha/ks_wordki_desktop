using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Wordki.Helpers;

namespace Wordki.Views.Dialogs {
  /// <summary>
  /// Interaction logic for ListDialog.xaml
  /// </summary>
  public partial class ListDialog : Window, INotifyPropertyChanged {

    private string _button1Label;
    private string _button2Label;
    private IEnumerable<object> _items;
    private string _selectedItem;
    private int _selectedIndex;

    public string Button1Label {
      get { return _button1Label; }
      set {
        if (_button1Label != value) {
          _button1Label = value;
          OnPropertyChanged();
        }
      }
    }
    public string Button2Label {
      get { return _button2Label; }
      set {
        if (_button2Label != value) {
          _button2Label = value;
          OnPropertyChanged();
        }
      }
    }
    public IEnumerable<object> Items {
      get { return _items; }
      set {
        if (_items != value) {
          _items = value;
          OnPropertyChanged();
        }
      }
    }
    public string SelectedItem {
      get { return _selectedItem; }
      set {
        if (_selectedItem != value) {
          _selectedItem = value;
          OnPropertyChanged();
        }
      }
    }
    public int SelectedIndex {
      get { return _selectedIndex; }
      set {
        if (_selectedIndex != value) {
          _selectedIndex = value;
          OnPropertyChanged();
        }
      }
    }
    private Style _itemStyle;
    public Style ItemStyle {
      get { return _itemStyle; }
      set {
        if (_itemStyle == value) return;
        _itemStyle = value;
        OnPropertyChanged();
      }
    }
    public IList SelectedItems { get; set; }
    public BuilderCommand Button1Command { get; set; }
    public BuilderCommand Button2Command { get; set; }
    public BuilderCommand SelectionChangedCommand { get; set; }

    public ListDialog() {
      InitializeComponent();
      SelectionChangedCommand = new BuilderCommand(SelectionChanged);
      DataContext = this;
      Owner = App.Current.MainWindow;
      Width = Owner.ActualWidth;
    }

    private void SelectionChanged(object obj) {
      if (obj == null)
        return;
      IList lList = (obj as IList);
      if (lList == null)
        return;
      SelectedItems = lList;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}
