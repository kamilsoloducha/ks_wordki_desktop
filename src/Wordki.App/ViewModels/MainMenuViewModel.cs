using AutoMapper;
using Oazachaosu.Core.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using WordkiModel;

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
            BackgroundWorkQueue queue = new BackgroundWorkQueue();
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
                    lList.Add((double)item);
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
            BackgroundWorkQueue queue = new BackgroundWorkQueue();
            queue.OnCompleted = () =>
            {
            };
            queue.AddWork(new ApiWork<IEnumerable<GroupDTO>>
            {
                Request = new GetGroupsRequest(UserManagerSingleton.Instence.User),
                OnCompletedFunc = AddGroupToDatabase,
                OnFailedFunc = OnFailed
            });
            queue.AddWork(new ApiWork<IEnumerable<WordDTO>>
            {
                Request = new GetWordsRequest(UserManagerSingleton.Instence.User),
                OnCompletedFunc = AddWordToDatabase,
                OnFailedFunc = OnFailed
            });
            queue.AddWork(new ApiWork<IEnumerable<ResultDTO>>
            {
                Request = new GetResultsRequest(UserManagerSingleton.Instence.User),
                OnCompletedFunc = AddResultToDatabase,
                OnFailedFunc = OnFailed
            });
            queue.Execute();

            RefreshInfo();
        }

        private void AddResultToDatabase(IEnumerable<ResultDTO> obj)
        {
            IMapper mapper = AutoMapperConfig.Instance;
            IDatabase database = DatabaseSingleton.Instance;
            foreach (ResultDTO resultDto in obj)
            {
                IResult result = mapper.Map<ResultDTO, IResult>(resultDto);
                IGroup group = database.Groups.FirstOrDefault(x => x.Id == resultDto.GroupId);
                if (result.State < 0)
                {
                    database.DeleteResult(result);
                }
                if (group.Words.Any(x => x.Id == result.Id))
                {
                    database.UpdateResult(result);
                }
                else
                {
                    database.AddResult(result);
                }
            }
        }

        private void AddWordToDatabase(IEnumerable<WordDTO> obj)
        {
            IMapper mapper = AutoMapperConfig.Instance;
            IDatabase database = DatabaseSingleton.Instance;
            foreach (WordDTO wordDto in obj)
            {
                IWord word = mapper.Map<WordDTO, IWord>(wordDto);
                IGroup group = database.Groups.FirstOrDefault(x => x.Id == wordDto.GroupId);
                if (wordDto.State < 0)
                {
                    database.DeleteWord(word);
                }
                if (group.Words.Any(x => x.Id == word.Id))
                {
                    database.UpdateWord(word);
                }
                else
                {
                    database.AddWord(word);
                }
            }
        }

        private void AddGroupToDatabase(IEnumerable<GroupDTO> obj)
        {
            IEnumerable<IGroup> groups = AutoMapperConfig.Instance.Map<IEnumerable<GroupDTO>, IEnumerable<IGroup>>(obj);
            IDatabase database = DatabaseSingleton.Instance;
            foreach (IGroup group in groups)
            {
                if (group.State < 0)
                {
                    database.DeleteGroup(group);
                }
                if (database.Groups.Any(x => x.Id == group.Id))
                {
                    database.UpdateGroup(group);
                }
                else
                {
                    database.AddGroup(group);
                }
            }
        }

        private void OnFailed(ErrorDTO obj)
        {

        }

        public override void Unloaded()
        {
        }
    }
}
