using System;
using System.Collections.ObjectModel;
using Wordki.Helpers;
using Wordki.Helpers.Command;
using Wordki.Models;
using Wordki.Models.Connector;

namespace Wordki.ViewModels
{
    public class LoginViewModel : LoginMainViewModel
    {

        #region Properties
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
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

        public System.Windows.Input.ICommand ListViewSelectedChangedCommand { get; set; }
        public System.Windows.Input.ICommand RemoveUserCommand { get; set; }
        public System.Windows.Input.ICommand LoginCommand { get; set; }

        #endregion

        public LoginViewModel()
        {
            LoginCommand = new BuilderCommand(Loging);
            ListViewSelectedChangedCommand = new BuilderCommand(ListViewSelectedChanged);
            RemoveUserCommand = new BuilderCommand(RemoveUser);
            Users = new ObservableCollection<User>();
        }


        public override void InitViewModel()
        {
            base.InitViewModel();
            InitUsers();
        }

        private void InitUsers()
        {
            Users.Clear();
            foreach (var user in Database.GetDatabase().GetUsers())
            {
                Users.Add(user);
            }
        }

        #region Commands
        private void RemoveUser(object obj)
        {
            User user = obj as User;
            if (user == null)
            {
                return;
            }
            if (Database.GetDatabase().DeleteUser(user))
            {
                Users.Remove(user);
            }
            else
            {
                Console.WriteLine("Błąd usuwania");
            }
        }

        private void ListViewSelectedChanged(object obj)
        {
            User user = obj as User;
            if (user == null)
            {
                return;
            }
            UserName = user.Name;
        }

        public void Loging(object obj)
        {
            if (Password == null)
                return;
            if (UserName == null)
                return;
            User lUser = UserManager.GetInstance().FindLoginedUser(UserName.Trim(), Password.Trim());
            if (lUser != null)
            {
                StartWithUser(lUser);
                return;
            }
            User user = new User
            {
                Name = UserName,
                Password = Password
            };
            CommandQueue<ICommand> lQueue = new CommandQueue<ICommand>();
            lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestLogin(user)) { OnCompleteCommand = OnLogin });
            lQueue.Execute();
        }

        protected override void ChangeState(object obj)
        {
            Switcher.GetSwitcher().Switch(Switcher.State.Register);
        }

        #endregion

        #region Methods

        private void OnLogin(ApiResponse response)
        {
            HandleResponse(response);
        }

        #endregion

    }
}
