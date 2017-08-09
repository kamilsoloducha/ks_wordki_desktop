using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wordki.Controls;
using Wordki.Helpers;
using Wordki.Models;

namespace Wordki.ViewModels {
  public class MainViewModel : INotifyPropertyChanged, IWindowsManagerListener {
    private int _selectedTab;

    public BuilderCommand BackCommand { get; set; }

    private ObservableCollection<ToastProperties> _toastList;
    private BuilderViewModel _builderViewModel;
    private MainMenuViewModel _mainMenuViewModel;
    private LoginViewModel _loginViewModel;
    private GroupManagerViewModel _groupManagerViewModel;
    private SettingsViewModel _settingsViewModel;
    private TeachViewModel _teachViewModel;
    private WordManageViewModel _wordManageViewModel;
    private ChartViewModel _chartViewModel;
    private SameWordsViewModel _sameWordsViewModel;

    public ObservableCollection<ToastProperties> ToastsList {
      get { return _toastList; }
      set {
        _toastList = value;
        NotifyPropertyChanged();
      }
    }
    public BuilderViewModel BuilderViewModel {
      get { return _builderViewModel; }
      set {
        _builderViewModel = value;
        NotifyPropertyChanged();
        _builderViewModel.InitViewModel();
      }
    }

    public MainMenuViewModel MainMenuViewModel {
      get { return _mainMenuViewModel; }
      set {
        _mainMenuViewModel = value;
        value.InitViewModel();
        NotifyPropertyChanged();
      }
    }

    public GroupManagerViewModel GroupManagerViewModel {
      get { return _groupManagerViewModel; }
      set {
        _groupManagerViewModel = value;
        value.InitViewModel();
        NotifyPropertyChanged();
      }
    }

    public LoginViewModel LoginViewModel {
      get { return _loginViewModel; }
      set {
        _loginViewModel = value;
        value.InitViewModel();
        NotifyPropertyChanged();
      }
    }

    public SettingsViewModel SettingsViewModel {
      get { return _settingsViewModel; }
      set {
        _settingsViewModel = value;
        value.InitViewModel();
        NotifyPropertyChanged();
      }
    }

    public TeachViewModel TeachViewModel {
      get { return _teachViewModel; }
      set {
        _teachViewModel = value;
        value.InitViewModel();
        NotifyPropertyChanged();
      }
    }

    public WordManageViewModel WordManageViewModel {
      get { return _wordManageViewModel; }
      set {
        _wordManageViewModel = value;
        value.InitViewModel();
        NotifyPropertyChanged();
      }
    }
    public ChartViewModel ChartViewModel {
      get { return _chartViewModel; }
      set {
        _chartViewModel = value;
        value.InitViewModel();
        NotifyPropertyChanged();
      }
    }
    public SameWordsViewModel SameWordsViewModel {
      get { return _sameWordsViewModel; }
      set {
        _sameWordsViewModel = value;
        value.InitViewModel();
        NotifyPropertyChanged();
      }
    }

    public Settings Settings { get; set; }

    public int SelectedTab {
      get { return _selectedTab; }
      private set {
        if (_selectedTab != value) {
          _selectedTab = value;
          NotifyPropertyChanged();
          InitViewModel(_selectedTab);
          Logger.LogInfo("Stan - {0}", value);
        }
      }
    }

    public WindowsManager WindowsManager { get; set; }

    public MainViewModel() {
      Settings = Settings.GetSettings();
      ToastsList = new ObservableCollection<ToastProperties>();
      WindowsManager = new WindowsManager();

      BackCommand = new BuilderCommand(Back);
    }

    private void Back(object obj) {
      WindowsManager.Back();
    }

    public void InitViewModel(int pViewModelIndex) {
      State lState = (State)pViewModelIndex;
      try {
        switch (lState) {
          case State.LOGIN:
            LoginViewModel = new LoginViewModel(this);
            break;
          case State.MAIN_MENU:
            MainMenuViewModel = new MainMenuViewModel(this);
            break;
          case State.BUILDER:
            BuilderViewModel = new BuilderViewModel(this);
            break;
          case State.GROUP_MANAGER:
            GroupManagerViewModel = new GroupManagerViewModel(this);
            break;
          case State.TEACHER:
            TeachViewModel = new TeachViewModel(this);
            break;
          case State.SETTINGS:
            SettingsViewModel = new SettingsViewModel(this);
            break;
          case State.WORD_MANAGER:
            WordManageViewModel = new WordManageViewModel(this);
            break;
          case State.CHART:
            ChartViewModel = new ChartViewModel(this);
            break;
          case State.SAME_WORDS:
            SameWordsViewModel = new SameWordsViewModel(this);
            break;
        }
      } catch (Exception lException) {
        Logger.LogError("{0} - {1} - {2}", "Blad w czasie inicjalizacji viewModelu", lState.ToString(), lException.Message);
      }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    public void ShowToast(string pMessage, ToastLevel pLevel) {
      try {
        string lMessage = pMessage;
        ImageSource lImageSource = null;
        Uri lUri = null;
        switch (pLevel) {
          case ToastLevel.Info: {
              lUri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Icons", "info.png"));
            }
            break;
          case ToastLevel.Alert: {
              lUri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Icons", "alert.png"));
            }
            break;
          case ToastLevel.Error: {
              lUri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Icons", "error.png"));
            }
            break;
        }
        if (lUri != null) {
          lImageSource = new BitmapImage(lUri);
          //lImageSource = SourceLoader.FlagsDictionary[Languages.English];
        }
        ToastsList.Add(new ToastProperties {
          ImageSource = lImageSource,
          Message = lMessage
        });
      } catch (Exception lException) {
        Logger.LogError("{0} - {1}", "MainViewModel.ShowToast", lException.Message);
      }
    }

    public void OnWindowsManagerUpdate(params object[] pParams) {
      if (pParams == null || !pParams.Any()) {
        Logger.LogError("{0} - {1}", "MainViewModel.OnWindowsManagerUpdate", "pParams == null");
        return;
      }
      State lNewState = (State)pParams[0];
      SelectedTab = (int)lNewState;
    }
  }
}
