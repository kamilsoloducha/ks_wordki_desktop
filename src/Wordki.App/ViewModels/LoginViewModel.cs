using Oazachaosu.Core.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Windows.Input;
using Util.Threads;
using Wordki.Database;
using Wordki.Helpers.AutoMapper;
using Wordki.Helpers.Connector.Requests;
using Wordki.Helpers.Connector.Work;
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

        public ICommand ListViewSelectedChangedCommand { get; set; }
        public ICommand RemoveUserCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        #endregion

        public LoginViewModel()
        {
            LoginCommand = new Util.BuilderCommand(Loging);
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

        public void Loging(object obj)
        {
            LoginAction();
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

        private WorkResult LoginAction()
        {
            if (Password == null)
                return new WorkResult();
            if (UserName == null)
                return new WorkResult();
            NHibernateHelper.DatabaseName = UserName;
            IUser user = DatabaseSingleton.Instance.GetUser(UserName, Password);
            if (user != null)
            {
                StartWithUser(user);
                return new WorkResult()
                {
                    Success = true,
                };
            }
            else
            {
                user = new User
                {
                    Name = UserName,
                    Password = Password,
                };
                ApiWork<UserDTO> loginRequest = new ApiWork<UserDTO>
                {
                    Request = new GetUserRequest(user),
                    OnCompletedFunc = LoginComplete,
                    OnFailedFunc = LoginFailed
                };
                BackgroundWorkQueue queue = new BackgroundWorkQueue();
                queue.AddWork(loginRequest);
                queue.Execute();
                return new WorkResult()
                {
                    Success = true,
                };
            }
        }

        private void LoginFailed(ErrorDTO error)
        {
            Console.WriteLine($"Error: {error.Code} \n" +
                $"{error.Message}");
            ShowInfoDialog($"Wystąpił błąd serwera w trakcie wykonywania żądania: '{error.Message}'.\nKod błędu: '{error.Code}'");
        }

        private void LoginComplete(UserDTO userDTO)
        {
            IUser user = AutoMapperConfig.Instance.Map<UserDTO, User>(userDTO);
            IDatabaseOrganizer databaseOrganizer = DatabaseOrganizerSingleton.Get();
            if (!databaseOrganizer.AddDatabase(user))
            {
                Console.WriteLine("Błąd dodawania bazy danych");
                return;
            }
            StartWithUser(user);
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
