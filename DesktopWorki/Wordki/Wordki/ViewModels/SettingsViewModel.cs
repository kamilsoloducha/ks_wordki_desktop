using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Wordki.Helpers;
using Wordki.Models;
using Wordki.Models.Connector;
using Wordki.Models.RemoteDatabase;
using Wordki.Views.Dialogs;
using Wordki.Helpers.Command;
using Wordki.Helpers.Notification;
using Wordki.Database;

namespace Wordki.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public Settings Settings { get; set; }
        public IDatabase Database { get; set; }

        public BuilderCommand BackCommand { get; set; }
        public BuilderCommand DefaultCommand { get; set; }
        public BuilderCommand SaveCommand { get; set; }
        public BuilderCommand LoginPasswordChangeCommand { get; set; }

        public BuilderCommand LogoutCommand { get; set; }
        public BuilderCommand SynchronizeCommand { get; set; }
        public BuilderCommand ClearDatabaseCommand { get; set; }

        public BuilderCommand AddShortCutCommand { get; set; }
        public BuilderCommand DeleteShortCutCommand { get; set; }
        public BuilderCommand EditShortCutCommand { get; set; }

        private int _themeSelectedIndex;
        private string _fontSizeSelectedIndex;
        private int _tempFontSize;

        public int ThemeSelectedIndex
        {
            get { return _themeSelectedIndex; }
            set
            {
                if (_themeSelectedIndex == value) return;
                _themeSelectedIndex = value;
                OnPropertyChanged();
            }
        }

        public string FontSizeSelectedIndex
        {
            get { return _fontSizeSelectedIndex; }
            set
            {
                if (_fontSizeSelectedIndex == value) return;
                _fontSizeSelectedIndex = value;
                OnPropertyChanged();
                int temp;
                if (int.TryParse(FontSizeSelectedIndex, out temp))
                {
                    TempFontSize = temp;
                }
            }
        }

        public int TempFontSize
        {
            get { return _tempFontSize; }
            set
            {
                if (_tempFontSize == value) return;
                _tempFontSize = value;
                OnPropertyChanged();
            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName == value) return;
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _loginDateTime;
        public string LoginDateTime
        {
            get { return _loginDateTime; }
            set
            {
                if (_loginDateTime == value) return;
                _loginDateTime = value;
                OnPropertyChanged();
            }
        }

        private string _downloadDateTime;
        public string DownloadDateTime
        {
            get { return _downloadDateTime; }
            set
            {
                if (_downloadDateTime == value) return;
                _downloadDateTime = value;
                OnPropertyChanged();
            }
        }

        private string _testString;

        public string TestString
        {
            get { return _testString; }
            set
            {
                if (_testString == value) return;
                _testString = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<System.Windows.Input.KeyBinding> _shortKeys;
        public ObservableCollection<System.Windows.Input.KeyBinding> ShortKeys
        {
            get { return _shortKeys; }
            set
            {
                if (_shortKeys == value) return;
                _shortKeys = value;
                OnPropertyChanged();
            }
        }

        public SettingsViewModel()
        {
            ActivateCommand();
            Settings = Settings.GetSettings();
            Database = DatabaseSingleton.GetDatabase();
        }

        public override void InitViewModel()
        {
            ThemeSelectedIndex = Settings.ApplicationStyle == ApplicationStyleEnum.Dark ? 1 : 0;
            FontSizeSelectedIndex = Settings.FontSize.ToString("D");
            UserName = UserManagerSingleton.Get().User.Name;
            LoginDateTime = UserManagerSingleton.Get().User.LastLoginDateTime.ToString("HH:mm:ss dd/MM/yyyy");
            DownloadDateTime = UserManagerSingleton.Get().User.DownloadTime.ToString("HH:mm:ss dd/MM/yyyy");
            Password = "***";
            TestString = "Napis";
            Settings.ShortCuts.CollectionChanged += ShortCutsOnCollectionChanged;
            ShortKeys = ShortCutsBindings.GetBindings(Settings.ShortCuts, new BuilderCommand(InsertKey));
        }

        public override void Back()
        {
            Settings.ShortCuts.CollectionChanged -= ShortCutsOnCollectionChanged;
        }

        private void ShortCutsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            ShortKeys = ShortCutsBindings.GetBindings(Settings.ShortCuts, new BuilderCommand(InsertKey));
        }

        private void InsertKey(object obj)
        {
            TestString += obj as string;
        }

        private void ActivateCommand()
        {
            BackCommand = new BuilderCommand(Back);
            DefaultCommand = new BuilderCommand(Default);
            SaveCommand = new BuilderCommand(Save);
            LoginPasswordChangeCommand = new BuilderCommand(LoginPasswordChange);

            LogoutCommand = new BuilderCommand(Logout);
            SynchronizeCommand = new BuilderCommand(Synchronize);
            ClearDatabaseCommand = new BuilderCommand(ClearDatabase);

            AddShortCutCommand = new BuilderCommand(AddShortCut);
            DeleteShortCutCommand = new BuilderCommand(DeleteShortCut);
            EditShortCutCommand = new BuilderCommand(EditShortCut);
        }

        private void EditShortCut(object obj)
        {

        }

        private void DeleteShortCut(object obj)
        {
            ForeignLetter foreignLetter = obj as ForeignLetter;
            if (foreignLetter == null)
            {
                return;
            }
            Settings.ShortCuts.Remove(foreignLetter);
        }

        private void AddShortCut(object obj)
        {

        }

        private void LoginPasswordChange(object obj)
        {
            TextBoxDialog dialog = new TextBoxDialog()
            {
                Message = "Podaj stare hasło.",
                OkCommand = new BuilderCommand(LoginPasswordChangeOk),
            };
            dialog.ShowDialog();
        }

        private void LoginPasswordChangeOk(object obj)
        {
            string oldPassword = obj as string;
            string hashPassword = Util.MD5Hash.GetMd5Hash(MD5.Create(), oldPassword);
            var user = new User()
            {
                Name = UserManagerSingleton.Get().User.Name,
                Password = Password,
            };
            if (hashPassword.Equals(UserManagerSingleton.Get().User.Password))
            {
                CommandQueue<ICommand> lQueue = new CommandQueue<ICommand>();
                lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestPutUser(user)) { OnCompleteCommand = UpdateLoginPasswordComplete });
                lQueue.Execute();
            }
        }

        private void UpdateLoginPasswordComplete(ApiResponse pResponse)
        {
            if (pResponse.IsError)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => NotificationFactory.Create().Show("Błąd"));
            }
            else
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => NotificationFactory.Create().Show("Zmieniono"));
            }
        }

        private void Back(object obj)
        {
            Back();
            Switcher.GetSwitcher().Back();
        }

        private void Save(object obj)
        {
            Settings.ChangeStyle(ThemeSelectedIndex);
            Settings.FontSize = TempFontSize;
            Settings.SaveSettings();
            Switcher.GetSwitcher().Back();
        }

        private void Default(object obj)
        {
            CommandQueue<ICommand> lQueue = ApiConnector.GetCommonGroupsQueue();
            lQueue.CreateDialog = false;
            lQueue.Execute();
            //Settings.ChangeStyle(ApplicationStyleEnum.Light);
            //Settings settings = Settings.GetSettings();
            //settings.FontSize = 20;
            //settings.SaveSettings();
            //InitViewModel();
        }

        private void Synchronize(object obj)
        {
            UserManagerSingleton.Get().User.DownloadTime = new DateTime(1991, 5, 20);
            CommandQueue<ICommand> lQueue = new CommandQueue<ICommand>();
            CommandQueue<ICommand> downloadQueue = RemoteDatabaseBase.GetRemoteDatabase(UserManagerSingleton.Get().User as User).GetDownloadQueue();
            foreach (ICommand command in downloadQueue.MainQueue)
            {
                lQueue.MainQueue.AddLast(command);
            }
            //lQueue.MainQueue.AddLast(new CommandApiRequest(new ApiRequestGetDateTime(UserManagerSingleton.Get().User as User)) { OnCompleteCommand = lDatabase.OnReadDateTime });
            CommandQueue<ICommand> uploadQueue = RemoteDatabaseBase.GetRemoteDatabase(UserManagerSingleton.Get().User as User).GetUploadQueue();
            foreach (ICommand command in uploadQueue.MainQueue)
            {
                lQueue.MainQueue.AddLast(command);
            }
            lQueue.OnQueueComplete += success =>
            {
                Database.RefreshDatabaseAsync();
                Database.LoadDatabaseAsync();
            };
            lQueue.Execute();
        }

        private void Logout(object obj)
        {
            Database.SaveDatabaseAsync();
            Settings.ResetSettings();
            Switcher.GetSwitcher().Reset();
        }

        private void ClearDatabase(object obj)
        {
            //Models.Database.GetDatabase().GroupsList.Clear();
            //Models.Database.ClearDatabase();
            Logout(null);
        }



        private void Correct(object obj)
        {
        }

        private void List(object obj)
        {
        }

        private void Results(object obj)
        {
        }

        private void Info(object obj)
        {
            InfoDialog lDialog = new InfoDialog();
            lDialog.DialogTitle = "jakis tytuł";
            lDialog.Message = "jakaś wiadomość\nktory zajmuje\nwiecej niz jedna linie";
            lDialog.ButtonCommand = new BuilderCommand(o => lDialog.Close());
            lDialog.ButtonLabel = "Ok";
            lDialog.ShowDialog();
        }

        private void YesNo(object obj)
        {
            YesNoDialog lDialog = new YesNoDialog();
            lDialog.Message = "jakaś wiadomość\nktory zajmuje\nwiecej niz jedna linie";
            lDialog.PositiveCommand = new BuilderCommand(o => lDialog.Close());
            lDialog.PositiveLabel = "Tak";
            lDialog.NegativeCommand = new BuilderCommand(o => lDialog.Close());
            lDialog.NegativeLabel = "Nie";
            lDialog.DialogTitle = "Jakis tytuł";
            lDialog.ShowDialog();
        }
    }
}
