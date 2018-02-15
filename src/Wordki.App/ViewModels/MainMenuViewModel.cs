using AutoMapper;
using Oazachaosu.Core.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Util;
using Util.Threads;
using Wordki.Database;
using Wordki.Helpers.AutoMapper;
using Wordki.Helpers.Connector.Requests;
using Wordki.Helpers.Connector.Work;
using Wordki.InteractionProvider;
using Wordki.Models;
using Wordki.ViewModels.Dialogs;

namespace Wordki.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {

        private bool isLoadFromCloud = false;

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

        public IResultOrganizer ResultCalculator { get; set; }
        public IDatabase Database { get; set; }

        #endregion

        public MainMenuViewModel()
        {
            TeachCommand = new BuilderCommand(Teach);
            BuilderCommand = new BuilderCommand(Builder);
            SettingsCommand = new BuilderCommand(Settings);
            ExitCommand = new BuilderCommand(Exit);

            Database = DatabaseSingleton.Instance;

            ResultCalculator = new ResultOrganizer
            {
                Groups = DatabaseSingleton.Instance.Groups,
            };
        }

        public override void InitViewModel(object parameter = null)
        {

        }

        public override void Back()
        {
        }

        #region Commands
        public BuilderCommand TeachCommand { get; set; }
        public BuilderCommand BuilderCommand { get; set; }
        public BuilderCommand SettingsCommand { get; set; }
        public BuilderCommand ExitCommand { get; set; }


        private void Teach()
        {
            Switcher.Switch(Helpers.Switcher.State.Groups);
        }

        private void Builder()
        {
            Switcher.Switch(Helpers.Switcher.State.Builder);
        }

        private void Settings()
        {
            Switcher.Switch(Helpers.Switcher.State.Settings);
        }

        private void Exit()
        {
            IInteractionProvider provider = new YesNoProvider
            {
                ViewModel = new YesNoDialogViewModel
                {
                    YesAction = ExitAction,
                    Message = "Czy chcesz wyjść z programu?",
                    NegativeLabel = "Nie",
                    PositiveLabel = "Tak",

                }
            };
            provider.Interact();
        }

        private void ExitAction()
        {
            if (!UserManagerSingleton.Instence.User.IsRegister)
            {
                Application.Current.Shutdown();
                return;
            }
            BackgroundQueueWithProgressDialog queue = new BackgroundQueueWithProgressDialog();
            ProcessProvider provider = new ProcessProvider()
            {
                ViewModel = new Dialogs.Progress.ProgressDialogViewModel()
                {
                    ButtonLabel = "Anuluj",
                    DialogTitle = "Zapisywanie zmian w chmurze...",
                    CanCanceled = true,
                }
            };
            queue.Dialog = provider;
            queue.OnCompleted = () =>
            {
                DatabaseSingleton.Instance.RefreshDatabase();
                Application.Current.Dispatcher.Invoke(Application.Current.Shutdown);
            };
            queue.OnFailed = Application.Current.Shutdown;
            queue.AddWork(new ApiWork<string>
            {
                Request = new PostGroupsRequest(UserManagerSingleton.Instence.User, new GroupsSender().GetModelToSend()),
            });
            queue.AddWork(new ApiWork<string>
            {
                Request = new PostWordsRequest(UserManagerSingleton.Instence.User, new WordsSender().GetModelToSend()),
            });
            queue.AddWork(new ApiWork<string>
            {
                Request = new PostResultsRequest(UserManagerSingleton.Instence.User, new ResultsSender().GetModelToSend()),
            });
            queue.Execute();
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
                foreach (var item in calculator.GetDrawerCount())
                {
                    lList.Add(item);
                }
                string lTeachTimeToday = Helpers.Util.GetAproximatedTimeFromSeconds(ResultCalculator.GetLessonTimeToday());
                string lTeachTime = Helpers.Util.GetAproximatedTimeFromSeconds(lDatabase.Groups.Sum(x => x.Results.Sum(y => y.TimeCount)));
                int lWordCount = lDatabase.Groups.Sum(x => x.Words.Count);
                int lResultCount = lDatabase.Groups.Sum(x => x.Results.Count);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TeachTimeToday = lTeachTimeToday;
                    TeachTime = lTeachTime;
                    WordCount = lWordCount;
                    ResultCount = lResultCount;
                    Values = lList;
                    MaxValue = lList.Sum();
                });
            });
        }

        public override void Loaded()
        {
            Login = UserManagerSingleton.Instence.User.Name;
            if (isLoadFromCloud)
            {
                RefreshInfo();
                return;
            }
            if (!UserManagerSingleton.Instence.User.IsRegister)
            {
                RefreshInfo();
                return;
            }
            BackgroundQueueWithProgressDialog queue = new BackgroundQueueWithProgressDialog();
            ProcessProvider provider = new ProcessProvider()
            {
                ViewModel = new Dialogs.Progress.ProgressDialogViewModel()
                {
                    ButtonLabel = "Anuluj",
                    DialogTitle = "Wczytywanie danych z chmury...",
                    CanCanceled = true,
                }
            };
            queue.Dialog = provider;
            queue.OnCompleted = () =>
            {
                RefreshInfo();
            };
            IMapper mapper = AutoMapperConfig.Instance;
            IDatabase database = DatabaseSingleton.Instance;
            queue.AddWork(new ApiWork<IEnumerable<GroupDTO>>
            {
                Request = new GetGroupsRequest(UserManagerSingleton.Instence.User),
                OnCompletedFunc = (groups) =>
                {
                    new GroupHandler(mapper, database).Handle(groups);
                },
                OnFailedFunc = OnFailed
            });
            queue.AddWork(new ApiWork<IEnumerable<WordDTO>>
            {
                Request = new GetWordsRequest(UserManagerSingleton.Instence.User),
                OnCompletedFunc = new WordHandler(mapper, database).Handle,
                OnFailedFunc = OnFailed
            });
            queue.AddWork(new ApiWork<IEnumerable<ResultDTO>>
            {
                Request = new GetResultsRequest(UserManagerSingleton.Instence.User),
                OnCompletedFunc = new ResultHandler(mapper, database).Handle,
                OnFailedFunc = OnFailed
            });
            queue.AddWork(new ApiWork<DateTime>
            {
                Request = new GetDateTimeRequest(UserManagerSingleton.Instence.User),
                OnCompletedFunc = async (dt) =>
                {
                    UserManagerSingleton.Instence.User.DownloadTime = dt;
                    await UserManagerSingleton.Instence.UpdateAsync();
                    isLoadFromCloud = true;
                    database.LoadDatabase();
                    RefreshInfo();
                },
                OnFailedFunc = OnFailed
            });
            queue.Execute();
        }

        private void OnFailed(ErrorDTO obj)
        {

        }

        public override void Unloaded()
        {
        }
    }
}
