using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Controls.ValueDescription;
using Wordki.Helpers;
using Wordki.Models;
using Wordki.Models.RemoteDatabase;
using System.Threading.Tasks;
using Wordki.Helpers.Command;
using Wordki.Database;

namespace Wordki.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        
        private string _statusBarText;
        private string _login;
        private ObservableCollection<double> _values;
        private double _maxValue;

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

        #region Properies
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

        #endregion

        public MainMenuViewModel()
        {
            TeachCommand = new BuilderCommand(Teach);
            BuilderCommand = new BuilderCommand(Builder);
            SettingsCommand = new BuilderCommand(Settings);
            ExitCommand = new BuilderCommand(Exit);

            ValueDescription l = new ValueDescription();
        }

        public override void InitViewModel()
        {
            Login = UserManagerSingleton.Get().User.Name;
            ReadDatabaseFromServer();
        }

        public override void Back()
        {
        }

        #region Commands
        public BuilderCommand TeachCommand { get; set; }
        public BuilderCommand BuilderCommand { get; set; }
        public BuilderCommand SettingsCommand { get; set; }
        public BuilderCommand ExitCommand { get; set; }

        private void Teach(object obj)
        {
            Switcher.GetSwitcher().Switch(Switcher.State.Groups);
        }

        private void Builder(object obj)
        {
            Switcher.GetSwitcher().Switch(Switcher.State.Builder);
        }

        private void Settings(object obj)
        {
            Switcher.GetSwitcher().Switch(Switcher.State.Settings);
        }

        private void Exit(object obj)
        {
            DatabaseSingleton.GetDatabase().SaveDatabaseAsync();
            CommandQueue<ICommand> lQueue = RemoteDatabaseBase.GetRemoteDatabase(UserManagerSingleton.Get().User as User).GetUploadQueue();
            lQueue.OnQueueComplete += success => Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
            lQueue.Execute();
        }
        #endregion

        private void RefreshInfo()
        {
            IDatabase lDatabase = DatabaseSingleton.GetDatabase();
            ObservableCollection<double> lList = new ObservableCollection<double>();
            string lTeachTimeToday = "to DO";//Helpers.Util.GetAproximatedTimeFromSeconds(lDatabase.GroupsList.Sum(x => x.GetLessonTime(DateTime.Now)));
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
        }

        private bool _needRead = true;
        private void ReadDatabaseFromServer()
        {
            if (!_needRead)
            {
                Task.Run(() => RefreshInfo());
                return;
            }
            CommandQueue<ICommand> lQueue = RemoteDatabaseBase.GetRemoteDatabase(UserManagerSingleton.Get().User as User).GetDownloadQueue();
            lQueue.OnQueueComplete += success => RefreshInfo();
            lQueue.Execute();
            _needRead = false;
        }
    }
}
