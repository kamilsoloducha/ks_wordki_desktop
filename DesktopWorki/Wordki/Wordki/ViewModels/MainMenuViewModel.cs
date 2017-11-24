using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Wordki.Models;
using System.Threading.Tasks;
using Wordki.Database;
using System.Windows.Input;
using Wordki.InteractionProvider;
using Util;
using Wordki.ViewModels.Dialogs;
using System.ComponentModel;

namespace Wordki.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {

        #region Properies

        private ObservableCollection<double> _values;
        public ObservableCollection<double> Values
        {
            get { return _values; }
            set
            {
                if (_values == value) return;
                _values = value;
                OnPropertyChanged();
            }
        }

        private double _maxValue;
        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (Math.Abs(_maxValue - value) < 0.1) return;
                _maxValue = value;
                OnPropertyChanged();
            }
        }

        private string _statusBarText;
        public string StatusBarText
        {
            get { return _statusBarText; }
            set
            {
                if (_statusBarText == value)
                {
                    return;
                }
                _statusBarText = value;
                OnPropertyChanged();
            }
        }

        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = "Witaj " + value;
                    OnPropertyChanged();
                }
            }
        }

        private string _teachTimeToday;
        public string TeachTimeToday
        {
            get { return _teachTimeToday; }
            set
            {
                if (_teachTimeToday == value) return;
                _teachTimeToday = value;
                OnPropertyChanged();
            }
        }

        private string _teachTime;
        public string TeachTime
        {
            get { return _teachTime; }
            set
            {
                if (_teachTime == value) return;
                _teachTime = value;
                OnPropertyChanged();
            }
        }

        private int _groupCount;
        public int GroupCount
        {
            get { return _groupCount; }
            set
            {
                if (_groupCount == value) return;
                _groupCount = value;
                OnPropertyChanged();
            }
        }

        private int _wordCount;
        public int WordCount
        {
            get { return _wordCount; }
            set
            {
                if (_wordCount == value) return;
                _wordCount = value;
                OnPropertyChanged();
            }
        }

        private int _resultCount;
        public int ResultCount
        {
            get { return _resultCount; }
            set
            {
                if (_resultCount == value) return;
                _resultCount = value;
                OnPropertyChanged();
            }
        }

        public IResultCalculator ResultCalculator { get; set; }

        #endregion

        public MainMenuViewModel()
        {
            TeachCommand = new BuilderCommand(Teach);
            BuilderCommand = new BuilderCommand(Builder);
            SettingsCommand = new BuilderCommand(Settings);
            ExitCommand = new BuilderCommand(Exit);
            SearchCommand = new BuilderCommand(Search);

            ResultCalculator = new ResultCalculator
            {
                Groups = DatabaseSingleton.Instance.Groups,
            };
        }

        public override void InitViewModel()
        {
            Login = UserManagerSingleton.Instence.User.Name;
            RefreshInfo();
        }

        public override void Back()
        {
        }

        #region Commands
        public BuilderCommand TeachCommand { get; set; }
        public BuilderCommand BuilderCommand { get; set; }
        public BuilderCommand SettingsCommand { get; set; }
        public BuilderCommand ExitCommand { get; set; }
        public ICommand SearchCommand { get; set; }


        private void Search(object obj)
        {
            ISearchProvider provider = new SearchProvider();
            provider.Interact();
        }

        private void Teach(object obj)
        {
            Switcher.Switch(Helpers.Switcher.State.Groups);
        }

        private void Builder(object obj)
        {
            Switcher.Switch(Helpers.Switcher.State.Builder);
        }

        private void Settings(object obj)
        {
            Switcher.Switch(Helpers.Switcher.State.Settings);
        }

        private void Exit(object obj)
        {
            IInteractionProvider provider = new YesNoProvider
            {
                ViewModel = new YesNoDialogViewModel
                {
                    YesAction = ExitAction,
                    Message = "Czy chcesz wyjść z programu?",
                    NegativeLabel = "Nie",
                    PositiveLabel = "Tak"
                }
            };
            provider.Interact();
        }

        private void ExitAction()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) => DatabaseSingleton.Instance.RefreshDatabase();
            worker.RunWorkerAsync();
            Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
        }
        #endregion

        private void RefreshInfo()
        {
            Task.Run(() =>
            {
                IDatabase lDatabase = DatabaseSingleton.Instance;
                ObservableCollection<double> lList = new ObservableCollection<double>();
                IWordCalculator calculator = new WordCalculator()
                {
                    Groups = lDatabase.Groups,
                };
                foreach (int drawer in calculator.GetDrawerCount())
                {
                    lList.Add(drawer);
                }
                string lTeachTimeToday = Helpers.Util.GetAproximatedTimeFromSeconds(ResultCalculator.GetLessonTimeToday());
                string lTeachTime = Helpers.Util.GetAproximatedTimeFromSeconds(lDatabase.Groups.Sum(x => x.Results.Sum(y => y.TimeCount)));
                int lGroupCount = lDatabase.Groups.Count;
                int lWordCount = lDatabase.Groups.Sum(x => x.Words.Count);
                int lResultCount = lDatabase.Groups.Sum(x => x.Results.Count);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TeachTimeToday = lTeachTimeToday;
                    TeachTime = lTeachTime;
                    GroupCount = lGroupCount;
                    WordCount = lWordCount;
                    ResultCount = lResultCount;
                    Values = lList;
                    MaxValue = lList.Sum();
                });
            });
        }
    }
}
