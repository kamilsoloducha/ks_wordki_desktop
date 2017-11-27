using Repository.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Windows.Input;
using Util.Threads;
using Wordki.Database;
using Wordki.Helpers;
using Wordki.InteractionProvider;
using Wordki.Models;
using Wordki.ViewModels.Dialogs;

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
            SimpleWork work = new SimpleWork();
            work.WorkFunc += LoginAction;
            BackgroundQueueWithProgressDialog worker = new BackgroundQueueWithProgressDialog();
            ProcessProvider provider = new ProcessProvider()
            {
                ViewModel = new Dialogs.Progress.ProgressDialogViewModel()
                {
                    ButtonLabel = "Anuluj",
                    DialogTitle = "Zapisuje",
                    CanCanceled = true,
                }
            };
            worker.Dialog = provider;
            worker.AddWork(work);
            worker.Execute();
            return;
        }

        protected override void ChangeState(object obj)
        {
            Switcher.Switch(Switcher.State.Register);
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
            IUser user = DatabaseSingleton.Instance.GetUser(UserName, GetHashedPassword());
            if (user != null)
            {
                StartWithUser(user);
                return new WorkResult()
                {
                    Success = true,
                };
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
                if (!databaseOrganizer.AddDatabase(user))
                {
                    Console.WriteLine("Błąd dodawania bazy danych");
                    return new WorkResult();
                }
                StartWithUser(user);
                return new WorkResult()
                {
                    Success = true,
                };
            }
        }
        #endregion
    }
}
