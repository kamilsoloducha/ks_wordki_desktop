using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Wordki.Models;

namespace Wordki.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SplitGroupDialog.xaml
    /// </summary>
    public partial class SplitGroupDialog : Window {

    public SplitGroupViewModel ViewModel { get; private set; }

    public SplitGroupDialog() {
      InitializeComponent();
      ViewModel = new SplitGroupViewModel(this);
      DataContext = ViewModel;
      Owner = App.Current.MainWindow;
      Width = Owner.ActualWidth;
    }

    public void SetGroupToSplit(Group pGroup) {
      ViewModel.Group = pGroup;
    }

    private void SplitGroupDialog_OnLoaded(object sender, RoutedEventArgs e) {
      Activate();
    }
  }

  public class SplitGroupViewModel : INotifyPropertyChanged {

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string pPropertyName) {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) {
        handler(this, new PropertyChangedEventArgs(pPropertyName));
      }
    }
    #endregion

    #region Properties
    private int _tabSelected;
    private int _percentage;
    private string _groupCount;
    private string _wordCount;

    public int TabSelected {
      get { return _tabSelected; }
      set {
        if (_tabSelected == value) return;
        _tabSelected = value;
        OnPropertyChanged("TabSelected");
      }
    }

    public int Percentage {
      get { return _percentage; }
      set {
        if (_percentage == value) return;
        _percentage = value;
        OnPropertyChanged("Percentage");
      }
    }

    public string GroupCount {
      get { return _groupCount; }
      set {
        if (_groupCount == value) return;
        _groupCount = value;
        OnPropertyChanged("GroupCount");
      }
    }

    public string WordCount {
      get { return _wordCount; }
      set {
        if (_wordCount == value) return;
        _wordCount = value;
        OnPropertyChanged("WordCount");
      }
    }

    public ICommand OkCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }
    public delegate void OkClickListener();
    public delegate void CancelClickListener();
    public OkClickListener OnOkClickListener { get; set; }
    public CancelClickListener OnCancelClickListener { get; set; }
    public Group Group { get; set; }
    private Window Parent { get; set; }

    #endregion

    public SplitGroupViewModel(Window pParent) {
      Parent = pParent;
      ActivateCommands();
    }

    private void ActivateCommands() {
      OkCommand = new Util.BuilderCommand(Ok);
      CancelCommand = new Util.BuilderCommand(Cancel);
    }

    private void Cancel(object obj) {
      if (OnCancelClickListener != null)
        OnCancelClickListener();
      Parent.Close();
    }

    private void Ok(object obj) {
      if (OnOkClickListener != null)
        OnOkClickListener();
      Parent.Close();
    }
  }
}
