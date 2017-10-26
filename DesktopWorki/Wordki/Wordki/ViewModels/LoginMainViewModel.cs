using System.Windows;
using Newtonsoft.Json;
using Wordki.Helpers;
using Wordki.Models;
using Wordki.Models.Connector;
using Wordki.Helpers.Notification;
using System.Windows.Input;
using Repository.Models;
using System;
using Wordki.Database;

namespace Wordki.ViewModels
{
    public abstract class LoginMainViewModel : ViewModelBase
    {
        
        #region Properties
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ExitCommand { get; set; }
        public ICommand ChangeStateCommand { get; set; }
        public ICommand LoadedWindowCommand { get; set; }

        #endregion

        #region Constructors

        protected LoginMainViewModel()
        {
            ExitCommand = new BuilderCommand(Exit);
            //LoadedWindowCommand = new BuilderCommand(LoadedWindow);
            ChangeStateCommand = new BuilderCommand(ChangeState);
        }

        #endregion

        public override void InitViewModel()
        {
            UserName = "";
            Password = "";
        }

        public override void Back()
        {
        }

        #region Commands

        protected abstract void ChangeState(object obj);

        private void Exit(object obj)
        {
            Application.Current.Shutdown();
        }

        //private void LoadedWindow(object obj)
        //{
        //    User lUser = UserManager.GetInstance().FindLoginedUser();
        //    if (lUser == null)
        //    {
        //        return;
        //    }
        //    CommandQueue<ICommand> queue = new CommandQueue<ICommand> { CreateDialog = true };
        //    queue.MainQueue.AddFirst(new SimpleCommand(() =>
        //    {
        //        IDatabase database = Database.GetDatabase();
        //        UserManager.GetInstance().User = lUser;
        //        return database.LoadDatabase();
        //    }));
        //    queue.OnQueueComplete += success =>
        //    {
        //        if (success)
        //        {
        //            StartWithUser(lUser);
        //        }
        //    };
        //    queue.Execute();
        //}

        #endregion

        #region Methods

        protected void StartWithUser(IUser user)
        {
            IUserManager userManager = UserManagerSingleton.Get();
            userManager.Set(user);
            user.LastLoginDateTime = DateTime.Now;
            userManager.Update();
            DatabaseSingleton.GetDatabase().LoadDatabaseAsync().ConfigureAwait(false);
            Start();
        }

        protected void HandleResponse(ApiResponse response)
        {
            if (response.IsError)
            {
                return;
            }
            User lUser = JsonConvert.DeserializeObject<User>(response.Message);
            lUser.IsLogin = true;
            lUser.IsRegister = true;
            IDatabase database = DatabaseSingleton.GetDatabase();
            //User dbUser = database.GetUserAsync(lUser.LocalId);
            //if (dbUser == null)
            //{
                //database.AddUser(lUser);
            //}
            //UserManager.GetInstance().User = lUser;
            StartWithUser(lUser);
        }

        private void Start()
        {
            //Logger.LogInfo("Loguje użytkownika: {0}", UserManager.GetInstance().User.GetStringFromObject());
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.Dispatcher.Invoke(() => NotificationFactory.Create().Show("Zalogowano"));
                Switcher.GetSwitcher().Switch(Switcher.State.Menu);
            });
        }

        #endregion

    }
}
