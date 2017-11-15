using Repository.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Wordki.Helpers;
using Wordki.Models;

namespace Wordki.Views.Dialogs {
  /// <summary>
  /// Interaction logic for LessonResultDialog.xaml
  /// </summary>
  public partial class LessonResultDialog : INotifyPropertyChanged {

    private string _correct;
    private string _accepted;
    private string _wrong;
    private string _time;
    private string _groupName;

    public string Time {
      get { return _time; }
      set {
        if (_time == value) return;
        _time = value;
        OnPropertyChanged();
      }
    }
    public string GroupName {
      get { return _groupName; }
      set {
        if (_groupName == value) return;
        _groupName = value;
        OnPropertyChanged();
      }
    }
    public string Correct {
      get { return _correct; }
      set {
        if (_correct != value) {
          _correct = value;
          OnPropertyChanged();
        }
      }
    }
    public string Accepted {
      get { return _accepted; }
      set {
        if (_accepted != value) {
          _accepted = value;
          OnPropertyChanged();
        }
      }
    }
    public string Wrong {
      get { return _wrong; }
      set {
        if (_wrong != value) {
          _wrong = value;
          OnPropertyChanged();
        }
      }
    }
    public BuilderCommand ButtonCommand { get; set; }

    private IList<IGroup> GroupList { get; set; }

    public LessonResultDialog(IList<IGroup> pGroupList) {
      InitializeComponent();
      DataContext = this;
      GroupList = pGroupList;
      int lCorrect = 0;
      int lAccepted = 0;
      int lWrong = 0;
      int lTime = 0;
      foreach (Group lItem in GroupList) {
        IResult lResult = lItem.Results.Last();
        lCorrect += lResult.Correct;
        lAccepted += lResult.Accepted;
        lWrong += lResult.Wrong;
        lTime += lResult.TimeCount;
      }
      Correct = lCorrect.ToString(CultureInfo.CurrentCulture);
      Accepted = lAccepted.ToString(CultureInfo.CurrentCulture);
      Wrong = lWrong.ToString(CultureInfo.CurrentCulture);
      Time = lTime.ToString(CultureInfo.CurrentCulture);
      Owner = Application.Current.MainWindow;
      Width = Owner.ActualWidth;

    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    private void LessonResultDialog_OnLoaded(object sender, RoutedEventArgs e) {
      Activate();
    }
  }
}
