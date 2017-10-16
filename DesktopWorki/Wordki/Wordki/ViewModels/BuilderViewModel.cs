using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Repository.Models.Language;
using Wordki.Helpers;
using Wordki.Helpers.GroupSplitter;
using Wordki.Models;
using Wordki.Models.RemoteDatabase;
using Wordki.Views.Dialogs;
using Wordki.Views.Dialogs.ListDialogs;
using Util;
using Repository.Helper;
using Repository.Models;
using Wordki.Database2;

namespace Wordki.ViewModels
{
    public enum EnableElementBuilder
    {
        Previous,
        Next,
        GroupName,
        Language1,
        Language2,
        Language1Comment,
        Language2Comment,
    }

    public class BuilderViewModel : ViewModelBase
    {

        private readonly object _groupsLock = new object();
        private string _nextGroupLabel;
        private string _previousGroupLabel;
        private bool _languages1IsFocused;
        private IGroup _selectedGroup;
        private IWord _selectedWord;

        #region Properies

        public IGroup SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (_selectedGroup != null && _selectedGroup.Equals(value))
                    return;
                UpdateGroup(_selectedGroup);
                _selectedGroup = value;
                OnPropertyChanged();
            }
        }
        public IWord SelectedWord
        {
            get { return _selectedWord; }
            set
            {
                if (_selectedWord != null && _selectedWord.Equals(value))
                    return;
                UpdateWord(_selectedWord);
                _selectedWord = value;
                OnPropertyChanged();
            }
        }
        public string NextGroupLabel
        {
            get { return _nextGroupLabel; }
            set
            {
                if (_nextGroupLabel == value)
                    return;
                _nextGroupLabel = value;
                OnPropertyChanged();
            }
        }
        public string PreviousGroupLabel
        {
            get { return _previousGroupLabel; }
            set
            {
                if (_previousGroupLabel == value)
                    return;
                _previousGroupLabel = value;
                OnPropertyChanged();
            }
        }
        public bool Language1IsFocused
        {
            get { return _languages1IsFocused; }
            set
            {
                if (_languages1IsFocused != value)
                {
                    _languages1IsFocused = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _groupCount;
        public int GroupCount
        {
            get { return _groupCount; }
            set
            {
                if (_groupCount == value)
                    return;
                _groupCount = value;
                OnPropertyChanged();
            }
        }

        private int? _groupNumber;
        public int? GroupNumber
        {
            get { return _groupNumber; }
            set
            {
                if (_groupNumber == value)
                    return;
                _groupNumber = value;
                OnPropertyChanged();
            }
        }

        private int? _drawer;
        public int? Drawer
        {
            get { return _drawer; }
            set
            {
                if (_drawer == value)
                    return;
                _drawer = value;
                OnPropertyChanged();
            }
        }

        private int? _wordNumber;
        public int? WordNumber
        {
            get { return _wordNumber; }
            set
            {
                if (_wordNumber == value)
                    return;
                _wordNumber = value;
                OnPropertyChanged();
            }
        }


        public System.Windows.Input.ICommand PreviousWordCommand { get; set; }
        public System.Windows.Input.ICommand NextWordCommand { get; set; }
        public System.Windows.Input.ICommand PreviousGroupCommand { get; set; }
        public System.Windows.Input.ICommand NextGroupCommand { get; set; }
        public System.Windows.Input.ICommand AddWordCommand { get; set; }
        public System.Windows.Input.ICommand RemoveWordCommand { get; set; }

        public System.Windows.Input.ICommand BackCommand { get; set; }

        public System.Windows.Input.ICommand DownloadGroupsNameCommand { get; set; }
        public System.Windows.Input.ICommand AddGroupCommand { get; set; }
        public System.Windows.Input.ICommand AddClipboardGroupCommand { get; set; }
        public System.Windows.Input.ICommand RemoveGroupCommand { get; set; }
        public System.Windows.Input.ICommand SplitGroupCommand { get; set; }
        public System.Windows.Input.ICommand ConnectGroupCommand { get; set; }

        public System.Windows.Input.ICommand FindSameWordCommand { get; set; }
        public System.Windows.Input.ICommand ShowWordsCommnad { get; set; }

        public System.Windows.Input.ICommand SwapLanguagesCommand { get; set; }
        public System.Windows.Input.ICommand SwapSingleWordCommand { get; set; }

        public System.Windows.Input.ICommand ChangeLanguage1Command { get; set; }
        public System.Windows.Input.ICommand ChangeLanguage2Command { get; set; }

        public System.Windows.Input.ICommand WordSelectionChangedCommand { get; set; }
        public System.Windows.Input.ICommand GroupSelectionChangedCommand { get; set; }
        public System.Windows.Input.ICommand AddGroupFromFileCommand { get; set; }

        public Models.IDatabase Database { get; set; }
        public ObservableDictionary<string, bool> EnableElementDirectory { get; set; }
        //private ClipboardHelper ClipboardHelper { get; set; }
        //private NotifyIcon NotifyIcon { get; set; }
        public Settings Settings { get; set; }

        #endregion

        public BuilderViewModel()
        {
            ActivateCommands();
            Database = Models.Database.GetDatabase();
            EnableElementDirectory = new ObservableDictionary<string, bool> {
                { EnableElementBuilder.Previous.ToString(), true },
                { EnableElementBuilder.Next.ToString(), true },
                { EnableElementBuilder.GroupName.ToString(), true },
                { EnableElementBuilder.Language1.ToString(), true },
                { EnableElementBuilder.Language2.ToString(), true },
                { EnableElementBuilder.Language1Comment.ToString(), true },
                { EnableElementBuilder.Language2Comment.ToString(), true }
            };
            //ClipboardHelper = new ClipboardHelper(App.Current.MainWindow);

            //NotifyIcon = new NotifyIcon();
            //ContextMenu lContextMenu = new ContextMenu();
            //lContextMenu.MenuItems.Add(new MenuItem("Powrót", (sender, args) => {
            //  NotifyIcon.Visible = false;
            //  ClipboardHelper.CloseCBViewer();
            //  //Parent.ShowToast("Zakończono tworzenie grupy", ToastLevel.Info);
            //  App.Current.MainWindow.WindowState = WindowState.Maximized;
            //}));
            //NotifyIcon.ContextMenu = lContextMenu;
            //NotifyIcon.Icon = SystemIcons.Application;//new Icon(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Icons", "Wordki.png"));

            BindingOperations.EnableCollectionSynchronization(Database.GroupsList, _groupsLock);
            Settings = Settings.GetSettings();
        }

        public override void InitViewModel()
        {
            Group lGroup = PackageStore.Get(0) as Group;
            if (lGroup != null)
            {
                SelectedGroup = lGroup;
                SetOnLastWordCurretGroup();
            }
            else
            {
                SetOnLastWord();
            }
            RefreshView();
        }

        public override void Back()
        {
            Database.SaveDatabase();
            var queue = RemoteDatabaseBase.GetRemoteDatabase(UserManagerSingleton.Get().User as User).GetUploadQueue();
            queue.CreateDialog = false;
            queue.Execute();
        }

        private async void UpdateWord(IWord pWord)
        {
            if (!await Database.UpdateWordAsync(pWord))
            {
                Logger.LogError("Błąd Updatu");
            }
        }

        private async void AddWord(IGroup pGroup, IWord pWord)
        {
            if (!await Database.AddWordAsync(pGroup, pWord))
            {
                Logger.LogError("Błąd dodanie słowa do bazy");
            }
        }

        private async void AddGroup(IGroup pGroup)
        {
            if (!await Database.AddGroupAsync(pGroup))
            {
                Logger.LogError("Błąd Dodania");
            }
        }

        private async void UpdateGroup(IGroup pGroup)
        {
            if (!await Database.UpdateGroupAsync(pGroup))
            {
                Logger.LogError("Błąd Updatu");
            }
        }

        private async void DeleteGroup(IGroup pGroup)
        {
            if (!await Database.DeleteGroupAsync(pGroup))
            {
                Logger.LogError("Błąd Usuwania");
            }
        }

        #region Commands
        private void ActivateCommands()
        {
            PreviousWordCommand = new Util.BuilderCommand(PreviousWord);
            NextWordCommand = new Util.BuilderCommand(NextWord);
            PreviousGroupCommand = new Util.BuilderCommand(PreviuosGroup);
            NextGroupCommand = new Util.BuilderCommand(NextGroup);

            AddWordCommand = new Util.BuilderCommand(AddWord);
            RemoveWordCommand = new Util.BuilderCommand(DeleteWord);
            AddGroupCommand = new Util.BuilderCommand(AddGroup);
            RemoveGroupCommand = new Util.BuilderCommand(RemoveGroup);

            DownloadGroupsNameCommand = new Util.BuilderCommand(DownloadGroupsName);
            BackCommand = new Util.BuilderCommand(Back);

            SplitGroupCommand = new Util.BuilderCommand(SplitGroup);
            ConnectGroupCommand = new Util.BuilderCommand(ConnectGroup);

            FindSameWordCommand = new Util.BuilderCommand(FindSame);
            ShowWordsCommnad = new Util.BuilderCommand(ShowWords);
            SwapLanguagesCommand = new Util.BuilderCommand(SwapWords);
            SwapSingleWordCommand = new Util.BuilderCommand(SwapSingleWord);

            ChangeLanguage1Command = new Util.BuilderCommand(ChangeLanguage1);
            ChangeLanguage2Command = new Util.BuilderCommand(ChangeLanguage2);
            AddClipboardGroupCommand = new Util.BuilderCommand(AddClipboardGroup);
            WordSelectionChangedCommand = new Util.BuilderCommand(WordSelectionChanged);
            GroupSelectionChangedCommand = new Util.BuilderCommand(GroupSelectionChanged);
            AddGroupFromFileCommand = new Util.BuilderCommand(AddGroupFromFile);
        }

        private void AddGroupFromFile(object obj)
        {
            Switcher.GetSwitcher().Switch(Switcher.State.BuildFromFile);
        }

        private void GroupSelectionChanged(object obj)
        {
            SetOnLastWordCurretGroup();
            RefreshView();
        }

        private void WordSelectionChanged(object obj)
        {
            RefreshView();
        }

        private void AddClipboardGroup(object obj)
        {
            //if (!ClipboardHelper.IsRunning) {
            //  NotifyIcon.Visible = true;
            //  NotifyIcon.ShowBalloonTip(2000, "Wordki", "Rozpoczęto tworzenie grupy ze schowka.\nSkopiuj słowa do schowka, które maja być zapisane w grupie", ToolTipIcon.Info);
            //  ClipboardHelper.InitCBViewer();
            //  Group lNewGroup = new Group();
            //  lNewGroup.Name = "Schowek" + DateTime.Now.Ticks % 10000;
            //  AddGroup(lNewGroup);
            //  ClipboardHelper.OnNewClipboardDelegate onNewClipboard = text => {
            //    Word lNewWord = new Word();
            //    lNewWord.GroupId = lNewGroup.Id;
            //    lNewWord.Language1 = text.Trim().ToLower();
            //    lNewGroup.WordsList.Add(lNewWord);
            //    NotifyIcon.ShowBalloonTip(2000, "Wordki", String.Format("Nowe słowo: {0}", lNewWord.Language1), ToolTipIcon.Info);
            //    SetOnLastWord();
            //  };
            //  ClipboardHelper.OnNewClipboard = onNewClipboard;
            //  //Parent.ShowToast("Rozpoczęto tworzenie grupy", ToastLevel.Info);
            //  App.Current.MainWindow.WindowState = WindowState.Minimized;
            //}
        }

        private void SwapWords(object obj)
        {
            if (Database.GroupsList.Count == 0)
                return;
            ILanguageSwaper swaper = new LanguageSwaper();
            swaper.Swap(SelectedGroup);
            SetWordLabels();
        }

        private void SwapSingleWord(object obj)
        {
            if (SelectedWord == null)
            {
                return;
            }
            ILanguageSwaper swaper = new LanguageSwaper();
            swaper.Swap(SelectedWord);
            Database.UpdateWordAsync(SelectedWord).ConfigureAwait(false);
        }

        private void ChangeLanguage2(object obj)
        {
            SelectLangauge(2);
        }

        private void ChangeLanguage1(object obj)
        {
            SelectLangauge(1);
        }

        private void ShowWords(object obj)
        {
            if (Database.GroupsList.Count == 0)
                return;
            long lGroupId = SelectedGroup.Id;
            PackageStore.Put(0, lGroupId);
            Switcher.GetSwitcher().Switch(Switcher.State.Words);
        }

        private void FindSame(object obj)
        {
            Switcher.GetSwitcher().Switch(Switcher.State.Same);
        }

        private void SplitGroup(object obj)
        {
            if (Database.GroupsList.Count == 0)
                return;
            SplitGroupDialog lDialog = new SplitGroupDialog();
            lDialog.ViewModel.OnOkClickListener += () =>
            {
                GroupSplitterBase lSpliter;
                int factor;
                switch (lDialog.ViewModel.TabSelected)
                {
                    case (int)SplitterEnum.Percentage:
                        {
                            lSpliter = new GroupSlitPercentage();
                            factor = lDialog.ViewModel.Percentage;
                        }
                        break;
                    case (int)SplitterEnum.GroupCount:
                        {
                            lSpliter = new GroupSplitGroupCount();
                            factor = Int32.Parse(lDialog.ViewModel.GroupCount);
                        }
                        break;
                    case (int)SplitterEnum.WordCount:
                        {
                            lSpliter = new GroupSplitWordCount();
                            factor = Int32.Parse(lDialog.ViewModel.WordCount);
                        }
                        break;
                    default:
                        return;
                }
                foreach (Group lGroup in lSpliter.Split(SelectedGroup, factor))
                {
                    AddGroup(lGroup);
                }
                Task.Run(() =>
                {
                    Database.SaveDatabase();
                    Application.Current.Dispatcher.Invoke(SetOnLastWord);
                });
            };
            lDialog.ShowDialog();
        }

        private void ConnectGroup(object obj)
        {
            try
            {

            }
            catch (Exception lException)
            {
                Logger.LogError("{0} - {1}", "BuilderViewModel.ConnectGroup", lException.Message);
            }
        }

        private void RemoveGroup(object obj)
        {
            if (SelectedGroup == null)
            {
                return;
            }
            YesNoDialog dialog = new YesNoDialog
            {
                DialogTitle = "Uwaga",
                Message = "Czy na pewno usunąć grupy?",
                PositiveLabel = "Tak",
                NegativeLabel = "Nie",
            };
            dialog.PositiveCommand = new Util.BuilderCommand(o =>
            {
                IGroup groupToDelete = SelectedGroup;
                int groupIndex = Database.GroupsList.IndexOf(SelectedGroup);
                SelectedGroup = Database.GroupsList.Count > groupIndex ? Database.GroupsList[groupIndex] : null;
                SelectedWord = SelectedGroup != null ? SelectedGroup.Words.LastOrDefault() : null;
                DeleteGroup(groupToDelete);
                dialog.Close();
                RefreshView();
            });
            dialog.NegativeCommand = new Util.BuilderCommand(o => dialog.Close());
            dialog.ShowDialog();
        }

        private void AddGroup(object obj)
        {
            Group lNewGroup = new Group();
            AddGroup(lNewGroup);
            SelectedGroup = lNewGroup;
            SetOnLastWordCurretGroup();
            RefreshView();
        }

        private void DownloadGroupsName(object obj)
        {
        }

        private string GroupToDownload { get; set; }
        private void OnGetCommonGroupNameRequestComplete()
        {

        }

        private void NextWord(object obj)
        {
            if (SelectedWord == null)
            {
                return;
            }
            int wordIndex = SelectedGroup.Words.IndexOf(SelectedWord);
            if (wordIndex < SelectedGroup.Words.Count - 1)
            {
                SelectedWord = SelectedGroup.Words[++wordIndex];
                RefreshView();
            }
        }

        private void PreviousWord(object obj)
        {
            if (SelectedWord == null)
            {
                return;
            }
            int wordIndex = SelectedGroup.Words.IndexOf(SelectedWord);
            if (wordIndex > 0)
            {
                SelectedWord = SelectedGroup.Words[--wordIndex];
                RefreshView();
            }
        }

        private void PreviuosGroup(object obj)
        {
            int lGroupIndex = Database.GroupsList.IndexOf(SelectedGroup);
            if (lGroupIndex > 0)
            {
                SelectedGroup = Database.GroupsList[--lGroupIndex];
                SetOnLastWordCurretGroup();
                RefreshView();
            }
        }

        private void NextGroup(object obj)
        {
            int lGroupIndex = Database.GroupsList.IndexOf(SelectedGroup);
            if (lGroupIndex < Database.GroupsList.Count - 1)
            {
                SelectedGroup = Database.GroupsList[++lGroupIndex];
                SetOnLastWordCurretGroup();
            }
        }

        private void Back(object obj)
        {
            Switcher.GetSwitcher().Back();
        }

        private void AddWord(object obj)
        {
            if (SelectedGroup == null)
            {
                AddGroupCommand.Execute(null);
            }
            if (SelectedGroup == null)
            {
                return;
            }
            Word lNewWord = new Word();
            lNewWord.GroupId = SelectedGroup.Id;
            AddWord(SelectedGroup, lNewWord);
            SelectedWord = lNewWord;
            Language1IsFocused = false;
            Language1IsFocused = true;
            RefreshView();
        }


        private async void DeleteWord(object obj)
        {
            //nie ma grup to wychodzi
            if (SelectedWord == null && SelectedGroup != null)
            {
                RemoveGroup(null);
                RefreshView();
                return;
            }
            if (await Database.DeleteWordAsync(SelectedGroup, SelectedWord))
            {
                SelectedWord = SelectedGroup.Words.LastOrDefault();
                if (SelectedWord == null)
                {
                    RemoveGroup(null);
                }
            }
            RefreshView();
        }

        private void SelectLangauge(int pLanguageIndex)
        {
            if (Database.GroupsList.Count == 0)
                return;
            LanguageListDialog lDialog = new LanguageListDialog
            {
                Items = LanguageFactory.GetLanguages(),
                Button1Label = "Wybierz"
            };
            lDialog.Button1Command = new Util.BuilderCommand(delegate
            {
                IGroup lGroup = SelectedGroup;
                LanguageType lSelectedLanguage = (LanguageType)lDialog.SelectedIndex;
                if (pLanguageIndex == 1)
                {
                    lGroup.Language1 = lSelectedLanguage;
                }
                else
                {
                    lGroup.Language2 = lSelectedLanguage;
                }
                lDialog.Close();
            });
            lDialog.Button2Label = "Anuluj";
            lDialog.Button2Command = new Util.BuilderCommand(delegate
            {
                lDialog.Close();
            });
            lDialog.ShowDialog();
        }
        #endregion

        #region Method

        public void RefreshView()
        {
            SetGroupNameLabel();
            SetWordLabels();
            SetNextGroupButton();
            SetPreviousGroupButton();
            SetInfo();
        }

        private void SetGroupNameLabel()
        {
            if (SelectedGroup == null)
            {
                UnableGroupName();
            }
            else
            {
                EnableGroupName();
            }
        }

        private void UnableGroupName()
        {
            EnableElementDirectory[EnableElementBuilder.GroupName.ToString()] = false;
        }

        private void EnableGroupName()
        {
            EnableElementDirectory[EnableElementBuilder.GroupName.ToString()] = true;
        }

        private void SetWordLabels()
        {
            if (SelectedWord == null)
            {
                UnableWord();
            }
            else
            {
                EnableWord();
            }
        }

        private void EnableWord()
        {
            EnableElementDirectory[EnableElementBuilder.Language1.ToString()] = true;
            EnableElementDirectory[EnableElementBuilder.Language2.ToString()] = true;
            EnableElementDirectory[EnableElementBuilder.Language1Comment.ToString()] = true;
            EnableElementDirectory[EnableElementBuilder.Language2Comment.ToString()] = true;
        }

        private void UnableWord()
        {
            EnableElementDirectory[EnableElementBuilder.Language1.ToString()] = false;
            EnableElementDirectory[EnableElementBuilder.Language2.ToString()] = false;
            EnableElementDirectory[EnableElementBuilder.Language1Comment.ToString()] = false;
            EnableElementDirectory[EnableElementBuilder.Language2Comment.ToString()] = false;
        }

        private void SetNextGroupButton()
        {
            if (SelectedGroup != null)
            {
                int lGroupIndex = Database.GroupsList.IndexOf(SelectedGroup);
                if (lGroupIndex < Database.GroupsList.Count - 1)
                {
                    NextGroupLabel = Database.GroupsList[lGroupIndex + 1].Name;
                    EnableElementDirectory[EnableElementBuilder.Next.ToString()] = true;
                }
                else
                {
                    NextGroupLabel = "";
                    EnableElementDirectory[EnableElementBuilder.Next.ToString()] = false;
                }
            }
            else
            {
                NextGroupLabel = "";
                EnableElementDirectory[EnableElementBuilder.Next.ToString()] = false;
            }
        }

        private void SetPreviousGroupButton()
        {
            if (SelectedGroup != null)
            {
                int lGroupIndex = Database.GroupsList.IndexOf(SelectedGroup);
                if (lGroupIndex > 0)
                {
                    PreviousGroupLabel = Database.GroupsList[lGroupIndex - 1].Name;
                    EnableElementDirectory[EnableElementBuilder.Previous.ToString()] = true;
                }
                else
                {
                    PreviousGroupLabel = "";
                    EnableElementDirectory[EnableElementBuilder.Previous.ToString()] = false;
                }
            }
            else
            {
                PreviousGroupLabel = "";
                EnableElementDirectory[EnableElementBuilder.Previous.ToString()] = false;
            }
        }

        private void SetInfo()
        {
            GroupCount = Database.GroupsList.Count;
            if (SelectedGroup != null)
            {
                GroupNumber = Database.GroupsList.IndexOf(SelectedGroup) + 1;
            }
            else
            {
                GroupNumber = null;
            }
            if (SelectedGroup != null && SelectedWord != null)
            {
                WordNumber = SelectedGroup.Words.IndexOf(SelectedWord) + 1;
                Drawer = SelectedWord.Drawer + 1;
            }
            else
            {
                WordNumber = null;
                Drawer = null;
            }
        }

        private void SetOnLastWord()
        {
            SelectedGroup = Database.GroupsList.LastOrDefault();
            if (SelectedGroup == null)
            {
                SelectedWord = null;
            }
            else
            {
                //RefreshWordsList();
                SelectedWord = SelectedGroup.Words.LastOrDefault();
            }
            RefreshView();
        }

        private void SetOnLastWordCurretGroup()
        {
            if (SelectedGroup == null)
            {
                SelectedWord = null;
            }
            else
            {
                SelectedWord = SelectedGroup.Words.LastOrDefault();
            }
            RefreshView();
        }
        #endregion

    }
}
