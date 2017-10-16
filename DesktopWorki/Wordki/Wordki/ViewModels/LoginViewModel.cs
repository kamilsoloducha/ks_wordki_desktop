using Repository.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wordki.Database2;
using Wordki.Helpers;
using Wordki.Helpers.Command;
using Wordki.Models;
using Wordki.Models.Connector;

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

        public System.Windows.Input.ICommand ListViewSelectedChangedCommand { get; set; }
        public System.Windows.Input.ICommand RemoveUserCommand { get; set; }
        public System.Windows.Input.ICommand LoginCommand { get; set; }

        #endregion

        public LoginViewModel()
        {
            LoginCommand = new BuilderCommand(Loging);
            ListViewSelectedChangedCommand = new BuilderCommand(ListViewSelectedChanged);
            Users = new ObservableCollection<string>();
        }


        public override void InitViewModel()
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
            if (Password == null)
                return;
            if (UserName == null)
                return;
            IUser user = DatabaseSingleton.GetDatabase().GetUserAsync(UserName, Password).Result;
            if (user != null)
            {
                IUserManager userManager = UserManagerSingleton.Get();
                userManager.Set(user);
                user.LastLoginDateTime = DateTime.Now;
                userManager.Update();
                StartWithUser();
                return;
                CommandQueue<ICommand> lQueue = new CommandQueue<ICommand>();
                lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestLogin(user as User)) { OnCompleteCommand = OnLogin });
                lQueue.Execute();
            }
            //inforamcja o zlym logowaniu
            Console.WriteLine("Błąd logowania");
            
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
