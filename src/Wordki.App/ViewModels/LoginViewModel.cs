using Oazachaosu.Core.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Windows.Input;
using Util.Threads;
using Wordki.Database;
using Wordki.Helpers.Connector.Requests;
using Wordki.Helpers.Connector.Work;
using Wordki.InteractionProvider;
using Wordki.Models;
using WordkiModel;

namespace Wordki.ViewModels
{
    public class LoginViewModel : LoginMainViewModel
    {

        #region Properties
        private IList<string> _users;
        public IList<string> Users
        {
            get { return _users; }
            set
            {
                if (_users != value)
                {
                    _users = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ListViewSelectedChangedCommand { get; }
        public ICommand RemoveUserCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand LoginLocalCommand { get; }

        #endregion

        public LoginViewModel()
        {
            LoginCommand = new Util.BuilderCommand(Login);
            LoginLocalCommand = new Util.BuilderCommand(LoginLocal);
            ListViewSelectedChangedCommand = new Util.BuilderCommand(ListViewSelectedChanged);
            Users = new ObservableCollection<string>();
        }


        public override void InitViewModel(object parameter = null)
        {
            base.InitViewModel();
            InitUsers();
        }

        private void InitUsers()
        {
            IDatabaseOrganizer organizer = new DatabaseOrganizer(NHibernateHelper.DirectoryPath);
            Users.Clear();
            foreach (var user in organizer.GetDatabases())
            {
                Users.Add(user);
            }
        }

        #region Commands
        private void ListViewSelectedChanged(object obj)
        {
            string user = obj as string;
            if (user == null)
            {
                return;
            }
            UserName = user;
        }

        private void LoginLocal()
        {
            if (!CheckUserName(UserName))
            {
                return;
            }
            SimpleWork loginRequest = new SimpleWork()
            {
                WorkFunc = () =>
                {
                    NHibernateHelper.DatabaseName = UserName;
                    IUser user = DatabaseSingleton.Instance.GetUser(UserName, Password);
                    if (user == null)
                    {
                        user = new User()
                        {
                            Name = UserName,
                            Password = Password,
                            CreateDateTime = DateTime.Now,
                            AllWords = true,
                            IsRegister = false,
                        };
                        DatabaseSingleton.Instance.AddUser(user);
                    }
                    StartWithUser(user);
                    return new WorkResult();
                },
            };
            BackgroundQueueWithProgressDialog queue = new BackgroundQueueWithProgressDialog();
            ProcessProvider provider = new ProcessProvider()
            {
                ViewModel = new Dialogs.Progress.ProgressDialogViewModel()
                {
                    ButtonLabel = "Anuluj",
                    DialogTitle = "Trwa logowanie do aplikacji...",
                    CanCanceled = true,
                }
            };
            queue.Dialog = provider;
            queue.AddWork(loginRequest);
            queue.Execute();
        }

        private void Login()
        {
            if (!CheckPassword(Password))
            {
                return;
            }
            if (!CheckUserName(UserName))
            {
                return;
            }
            IUser user = new User
            {
                Name = UserName,
                Password = Password,
            };
            ApiWork<UserDTO> loginRequest = new ApiWork<UserDTO>
            {
                Request = new GetUserRequest(user),
                OnCompletedFunc = OnUserRequestCompete,
                OnFailedFunc = LoginFailed
            };
            BackgroundQueueWithProgressDialog queue = new BackgroundQueueWithProgressDialog();
            ProcessProvider provider = new ProcessProvider()
            {
                ViewModel = new Dialogs.Progress.ProgressDialogViewModel()
                {
                    ButtonLabel = "Anuluj",
                    DialogTitle = "Trwa logowanie do aplikacji...",
                    CanCanceled = true,
                }
            };
            queue.Dialog = provider;
            queue.AddWork(loginRequest);
            queue.Execute();
        }

        protected override void ChangeState(object obj)
        {
            Switcher.Switch(Helpers.Switcher.State.Register);
        }

        #endregion

        #region Methods

        private string GetHashedPassword()
        {
            return Util.MD5Hash.GetMd5Hash(MD5.Create(), Password);
        }

        private void LoginFailed(ErrorDTO error)
        {
            Console.WriteLine($"Error: {error.Code} \n" +
                $"{error.Message}");
            ShowInfoDialog($"Wystąpił błąd serwera w trakcie wykonywania żądania: '{error.Message}'.\nKod błędu: '{error.Code}'");
        }

        public override void Loaded()
        {
            base.Loaded();
            IDatabaseOrganizer organizer = new DatabaseOrganizer(NHibernateHelper.DirectoryPath);
            Users.Clear();
            foreach (var user in organizer.GetDatabases())
            {
                Users.Add(user);
            }
        }

        public override void Unloaded()
        {
        }
        #endregion
    }
}
