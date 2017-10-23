using Repository.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Util.Threads;
using Wordki.Database;
using Wordki.Helpers;
using Wordki.Helpers.Command;
using Wordki.Models;
using Wordki.Models.Connector;
using Wordki.Views.Dialogs.Progress;

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
            SimpleAsyncWork work = new SimpleAsyncWork();
            work.WorkFunc += LoginAction;
            BackgroundQueueWithProgressDialog worker = new BackgroundQueueWithProgressDialog();
            ProgressDialog dialog = new ProgressDialog();
            dialog.ViewModel = new Dialogs.Progress.ProgressDialogViewModel()
            {
                ButtonLabel = "Anuluj",
                DialogTitle = "Loguje",
                CanCanceled = true,
            };
            worker.Dialog = dialog;
            worker.AddWork(work);
            worker.OnCompleted += () => { Console.WriteLine("OnCompleted"); };
            worker.OnCanceled += () => { Console.WriteLine("OnCanceled"); };
            worker.OnFailed += () => { Console.WriteLine("OnFailed"); };
            worker.Execute();
            return;
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


        private string GetHashedPassword()
        {
            return Util.MD5Hash.GetMd5Hash(MD5.Create(), Password);
        }

        private async Task<bool> LoginAction()
        {
            if (Password == null)
                return false;
            if (UserName == null)
                return false;
            NHibernateHelper.DatabaseName = UserName;
            IUser user = await DatabaseSingleton.GetDatabase().GetUserAsync(UserName, GetHashedPassword());
            if (user != null)
            {
                StartWithUser(user);
                return true;
                //CommandQueue<ICommand> lQueue = new CommandQueue<ICommand>();
                //lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestLogin(user as User)) { OnCompleteCommand = OnLogin });
                //lQueue.Execute();
            }
            else
            {
                IDatabaseOrganizer databaseOrganizer = DatabaseOrganizerSingleton.Get();
                user = new User()
                {
                    LocalId = DateTime.Now.Ticks,
                    Name = UserName,
                    Password = GetHashedPassword(),
                    CreateDateTime = DateTime.Now,

                };
                if (!await databaseOrganizer.AddDatabaseAsync(user))
                {
                    Console.WriteLine("Błąd dodawania bazy danych");
                    return false;
                }
                StartWithUser(user);
                return true;
            }
        }
        #endregion

    }


    public class TestWork : IWork
    {
        public bool Execute()
        {
            System.Threading.Thread.Sleep(2000);
            return true;
        }

        public bool Initialize()
        {
            return true;
        }

        public void OnCanceled()
        {
        }

        public void OnCompleted()
        {
        }

        public void OnFailed()
        {
        }
    }
}
