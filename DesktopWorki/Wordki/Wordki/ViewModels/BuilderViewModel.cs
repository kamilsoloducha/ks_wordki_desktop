using System;
using System.Linq;
using System.Windows.Data;
using Repository.Models.Language;
using Wordki.Helpers;
using Wordki.Helpers.GroupSplitter;
using Wordki.Models;
using Wordki.Views.Dialogs;
using Wordki.Views.Dialogs.ListDialogs;
using Util;
using Repository.Helper;
using Repository.Models;
using Wordki.Database;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections;
using Wordki.Helpers.GroupConnector;
using Util.Threads;
using Wordki.Views.Dialogs.Progress;
using Wordki.Models.Connector;
using Wordki.Helpers.Connector;
using Wordki.Helpers.Connector.Requests;
using Wordki.ViewModels.Dialogs;
using System.Windows;
using System.Text;

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
        private readonly object _wordLock = new object();

        #region Properies

        private IGroup _selectedGroup;
        public IGroup SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (_selectedGroup != null && _selectedGroup.Equals(value))
                    return;
                UpdateGroup(_selectedGroup);
                _selectedGroup = value;
                UpdateWords();
                OnPropertyChanged();
            }
        }



        private IWord _selectedWord;
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

        private string _nextGroupLabel;
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

        private string _previousGroupLabel;
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

        private bool _languages1IsFocused;
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

        public ICommand PreviousWordCommand { get; set; }
        public ICommand NextWordCommand { get; set; }
        public ICommand PreviousGroupCommand { get; set; }
        public ICommand NextGroupCommand { get; set; }
        public ICommand AddWordCommand { get; set; }
        public ICommand RemoveWordCommand { get; set; }
        public ICommand CheckUncheckWordCommand { get; set; }

        public ICommand BackCommand { get; set; }

        public ICommand DownloadGroupsNameCommand { get; set; }
        public ICommand AddGroupCommand { get; set; }
        public ICommand AddClipboardGroupCommand { get; set; }
        public ICommand RemoveGroupCommand { get; set; }
        public ICommand SplitGroupCommand { get; set; }
        public ICommand ConnectGroupCommand { get; set; }

        public ICommand FindSameWordCommand { get; set; }
        public ICommand ShowWordsCommnad { get; set; }

        public ICommand SwapLanguagesCommand { get; set; }
        public ICommand SwapSingleWordCommand { get; set; }

        public ICommand ChangeLanguage1Command { get; set; }
        public ICommand ChangeLanguage2Command { get; set; }

        public ICommand WordSelectionChangedCommand { get; set; }
        public ICommand GroupSelectionChangedCommand { get; set; }
        public ICommand AddGroupFromFileCommand { get; set; }
        public ICommand TranslateWordCommnad { get; set; }

        public IDatabase Database { get; set; }
        public ObservableDictionary<string, bool> EnableElementDirectory { get; set; }


        public ObservableCollection<IWord> Words { get; set; }


        //private ClipboardHelper ClipboardHelper { get; set; }
        //private NotifyIcon NotifyIcon { get; set; }
        public Settings Settings { get; set; }

        #endregion

        public BuilderViewModel()
        {
            ActivateCommands();
            Database = DatabaseSingleton.GetDatabase();
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

            BindingOperations.EnableCollectionSynchronization(Database.Groups, _groupsLock);
            Words = new ObservableCollection<IWord>();
            BindingOperations.EnableCollectionSynchronization(Words, _wordLock);
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
            UpdateGroup(SelectedGroup);
            UpdateWord(SelectedWord);
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
            CheckUncheckWordCommand = new Util.BuilderCommand(CheckUncheckWord);
            TranslateWordCommnad = new Util.BuilderCommand(TranslateWord);

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
            if (Database.Groups.Count == 0)
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
            if (Database.Groups.Count == 0)
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
            if (Database.Groups.Count == 0)
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
                Database.SaveDatabaseAsync();
                SetOnLastWord();
            };
            lDialog.ShowDialog();
        }

        private void ConnectGroup(object obj)
        {
            if (obj == null)
            {
                return;
            }
            IList list = obj as IList;
            if (list == null)
            {
                return;
            }
            IEnumerable<IGroup> groups = list.Cast<IGroup>();
            if (groups == null)
            {
                return;
            }
            IGroupConnector connector = new GroupConnector();
            if (!connector.Connect(groups.ToList()) && connector.DestinationGroup != null)
            {
                return;
            }
            foreach (IGroup group in groups.Where(x => x.Words.Count == 0))
            {
                DeleteGroup(group);
            }
            SelectedGroup = connector.DestinationGroup;
            UpdateWords();
            RefreshView();
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
            dialog.PositiveCommand = new Util.BuilderCommand(async o =>
            {
                int groupIndex = Database.Groups.IndexOf(SelectedGroup);
                SelectedGroup = Database.Groups.Count > groupIndex ? Database.Groups[groupIndex] : null;
                SelectedWord = SelectedGroup != null ? SelectedGroup.Words.LastOrDefault() : null;
                await Database.DeleteGroupAsync(SelectedGroup);
                dialog.Close();
                RefreshView();
            });
            dialog.NegativeCommand = new Util.BuilderCommand(o => dialog.Close());
            dialog.ShowDialog();
        }

        private void CheckUncheckWord(object obj)
        {
            if (SelectedWord == null)
            {
                return;
            }
            SelectedWord.Checked = !SelectedWord.Checked;
        }

        public void TranslateWord(object obj)
        {
            SimpleWork work = new SimpleWork();
            work.WorkFunc += SendRequestForWordTranslate;
            BackgroundQueueWithProgressDialog worker = new BackgroundQueueWithProgressDialog();
            ProgressDialog dialog = new ProgressDialog();
            dialog.ViewModel = new Dialogs.Progress.ProgressDialogViewModel()
            {
                DialogTitle = "Tłumaczę...",
                ButtonLabel = "Anuluj",
                CanCanceled = true,
            };
            worker.Dialog = dialog;
            worker.AddWork(work);
            worker.Execute();
        }

        private void AddGroup(object obj)
        {
            IGroup group = new Group();
            AddGroup_(group);
            SelectedGroup = group;
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
            int lGroupIndex = Database.Groups.IndexOf(SelectedGroup);
            if (lGroupIndex > 0)
            {
                SelectedGroup = Database.Groups[--lGroupIndex];
                SetOnLastWordCurretGroup();
                RefreshView();
            }
        }

        private void NextGroup(object obj)
        {
            int lGroupIndex = Database.Groups.IndexOf(SelectedGroup);
            if (lGroupIndex < Database.Groups.Count - 1)
            {
                SelectedGroup = Database.Groups[++lGroupIndex];
                SetOnLastWordCurretGroup();
            }
        }

        private void Back(object obj)
        {
            UpdateGroup(SelectedGroup);
            UpdateWord(SelectedWord);
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
            Word word = new Word();
            SelectedGroup.AddWord(word);
            Words.Add(word);
            AddWord_(word);
            SelectedWord = word;
            Language1IsFocused = false;
            Language1IsFocused = true;
            RefreshView();
        }


        private async void DeleteWord(object obj)
        {
            if (SelectedWord == null && SelectedGroup != null)
            {
                RemoveGroup(null);
                RefreshView();
                return;
            }
            if (await Database.DeleteWordAsync(SelectedWord))
            {
                Words.Remove(SelectedWord);
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
            if (Database.Groups.Count == 0)
                return;
            LanguageListDialog lDialog = new LanguageListDialog
            {
                Items = LanguageFactory.GetLanguages(),
                Button1Label = "Wybierz"
            };
            lDialog.Button1Command = new Util.BuilderCommand(delegate
            {
                LanguageType lSelectedLanguage = (LanguageType)lDialog.SelectedIndex;
                if (pLanguageIndex == 1)
                {
                    SelectedGroup.Language1 = lSelectedLanguage;
                }
                else
                {
                    SelectedGroup.Language2 = lSelectedLanguage;
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

        private void RefreshView()
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
                int lGroupIndex = Database.Groups.IndexOf(SelectedGroup);
                if (lGroupIndex < Database.Groups.Count - 1)
                {
                    NextGroupLabel = Database.Groups[lGroupIndex + 1].Name;
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
                int lGroupIndex = Database.Groups.IndexOf(SelectedGroup);
                if (lGroupIndex > 0)
                {
                    PreviousGroupLabel = Database.Groups[lGroupIndex - 1].Name;
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
            if (SelectedGroup != null)
            {
                GroupNumber = Database.Groups.IndexOf(SelectedGroup) + 1;
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
            SelectedGroup = Database.Groups.LastOrDefault();
            if (SelectedGroup != null)
            {
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

        private void UpdateWords()
        {
            if (SelectedGroup == null)
            {
                return;
            }
            Words.Clear();
            foreach (IWord word in SelectedGroup.Words)
            {
                Words.Add(word);
            }
        }

        private WorkResult SendRequestForWordTranslate()
        {
            IRequest request = new TranslationRequest(new Models.Translator.RequestBag
            {
                From = LanguageFactory.GetLanguage(SelectedGroup.Language1).Code,
                To = LanguageFactory.GetLanguage(SelectedGroup.Language2).Code,
                Word = SelectedWord.Language1
            });
            IConnector<TranslationResponse> connector = new SimpleConnector<TranslationResponse>();
            connector.Parser = new TranslationParser();
            TranslationResponse response = null;
            try
            {
                response = connector.SendRequest(request);
            }
            catch (Exception e)
            {
                LoggerSingleton.LogError(e.Message);
                return new WorkResult()
                {
                    Success = false,
                };
            }
            if (response != null)
            {
                List<string> items = new List<string>();
                foreach (var item in response.TranslationWord.Packages)
                {
                    if (item.Translation == null)
                    {
                        continue;
                    }
                    items.Add(item.Translation.Text);
                }

                TranslationListDialogViewModel viewModel = new TranslationListDialogViewModel(items);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TranslationListDialog dialog = new TranslationListDialog()
                    {
                        ViewModel = viewModel,
                    };
                    dialog.ShowDialog();
                    if (viewModel.Canceled)
                    {
                        return;
                    }
                    IEnumerable<string> selectedItems = viewModel.Items.Where(x => x.Approved).Select(x => x.Translation);
                    StringBuilder builder = new StringBuilder();
                    foreach(string item in selectedItems)
                    {
                        builder.Append(item).Append(", ");
                    }
                    builder.Remove(builder.Length - 2, 2);
                    SelectedWord.Language2 = builder.ToString();
                });
            }
            return new WorkResult()
            {
                Result = response,
                Success = true,
            };
        }

        private async void UpdateWord(IWord pWord)
        {
            if (!await Database.UpdateWordAsync(pWord))
            {
                LoggerSingleton.LogError("Błąd Updatu");
            }
        }

        private async void AddWord_(IWord pWord)
        {
            if (!await Database.AddWordAsync(pWord))
            {
                LoggerSingleton.LogError("Błąd Dodania");
            }
        }

        private async void AddGroup_(IGroup pGroup)
        {
            if (!await Database.AddGroupAsync(pGroup))
            {
                LoggerSingleton.LogError("Błąd Dodania");
            }
        }

        private async void UpdateGroup(IGroup pGroup)
        {
            if (!await Database.UpdateGroupAsync(pGroup))
            {
                LoggerSingleton.LogError("Błąd Updatu");
            }
        }

        private async void DeleteGroup(IGroup pGroup)
        {
            if (!await Database.DeleteGroupAsync(pGroup))
            {
                LoggerSingleton.LogError("Błąd Usuwania");
            }
        }
        #endregion

    }
}
