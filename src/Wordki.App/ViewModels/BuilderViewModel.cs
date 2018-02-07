using WordkiModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Util.Collections;
using Util.Threads;
using Wordki.Commands;
using Wordki.Database;
using Wordki.Helpers;
using Wordki.Helpers.Connector;
using Wordki.Helpers.Connector.Requests;
using Wordki.Helpers.GroupConnector;
using Wordki.Helpers.GroupSplitter;
using Wordki.InteractionProvider;
using Wordki.Models;
using Wordki.ViewModels.Dialogs;
using Wordki.Views.Dialogs;
using Oazachaosu.Core.Common;
using NLog;

namespace Wordki.ViewModels
{
    public class BuilderViewModel : ViewModelBase, IGroupSelectable, IWordSelectable
    {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
                {
                    return;
                }
                UpdateGroup(_selectedGroup);
                if (_selectedGroup != null)
                {
                    BindingOperations.DisableCollectionSynchronization(_selectedGroup.Words);
                }
                _selectedGroup = value;
                if (_selectedGroup != null)
                {
                    BindingOperations.EnableCollectionSynchronization(_selectedGroup.Words, _wordLock);
                }
                GroupNext = Database.Groups.Next(_selectedGroup);
                GroupPrevious = Database.Groups.Previous(_selectedGroup);
                OnPropertyChanged();
            }
        }

        private IGroup previousGroup;
        public IGroup GroupPrevious
        {
            get { return previousGroup; }
            set
            {
                if (previousGroup == value)
                {
                    return;
                }
                previousGroup = value;
                OnPropertyChanged();
            }
        }

        private IGroup nextGroup;
        public IGroup GroupNext
        {
            get { return nextGroup; }
            set
            {
                if (nextGroup == value)
                {
                    return;
                }
                nextGroup = value;
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

        private bool languages2IsFocused;
        public bool Language2IsFocused
        {
            get { return languages2IsFocused; }
            set
            {
                if (languages2IsFocused == value)
                {
                    return;
                }
                languages2IsFocused = value;
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

        public ICommand GroupSelectionChangedCommand { get; set; }
        public ICommand TranslateWordCommnad { get; set; }

        public IDatabase Database { get; set; }

        public Settings Settings { get; set; }

        #endregion

        public BuilderViewModel()
        {
            PreviousWordCommand = new Util.BuilderCommand(new SelectPreviousWordAction(this, this).Action);

            NextWordCommand = new Util.BuilderCommand(new SelectNextWordAction(this, this).Action);

            PreviousGroupCommand = new Util.CommandQueue(new Action[]{
                new SelectPreviousGroupAction(this, DatabaseSingleton.Instance).Action,
                new SelectLastWordAction(this, this).Action,
            });

            NextGroupCommand = new Util.CommandQueue(new Action[]{
                new SelectNextGroupAction(this, DatabaseSingleton.Instance).Action,
                new SelectLastWordAction(this, this).Action,
            });

            AddWordCommand = new Util.CommandQueue(new[] {
                new AddWordAction(this, this, DatabaseSingleton.Instance).Action,
                new SelectLastWordAction(this, this).Action,
            });

            RemoveWordCommand = new Util.CommandQueue(new[]
            {
                new RemoveWordAction(this, this, DatabaseSingleton.Instance).Action,
            });

            AddGroupCommand = new Util.CommandQueue(new[]
            {
                new AddGroupAction(this, DatabaseSingleton.Instance).Action,
                new SelectLastGroupAction(this, this, DatabaseSingleton.Instance).Action
            });

            RemoveGroupCommand = new Util.BuilderCommand((obj) => RemoveGroup(obj as IGroup));

            CheckUncheckWordCommand = new Util.BuilderCommand((obj) => ActionsSingleton<CheckUncheckAction>.Instance.Action(obj as IWord));
            TranslateWordCommnad = new Util.BuilderCommand(TranslateWord);

            BackCommand = new Util.BuilderCommand(BackAction);

            SplitGroupCommand = new Util.BuilderCommand((obj) => SplitGroup(obj as IGroup));
            ConnectGroupCommand = new Util.BuilderCommand(ConnectGroup);

            FindSameWordCommand = new Util.BuilderCommand(FindSame);
            ShowWordsCommnad = new Util.BuilderCommand(ShowWords);
            SwapLanguagesCommand = new Util.BuilderCommand((obj) => ActionsSingleton<SwapWordsInGroupAction>.Instance.Action(obj as IGroup));
            SwapSingleWordCommand = new Util.BuilderCommand((obj) => ActionsSingleton<SwapWordAction>.Instance.Action(obj as IWord));

            ChangeLanguage1Command = new Util.BuilderCommand((obj) => ChangeLanguage(obj as IGroup, 1));
            ChangeLanguage2Command = new Util.BuilderCommand((obj) => ChangeLanguage(obj as IGroup, 2));
            AddClipboardGroupCommand = new Util.BuilderCommand(AddClipboardGroup);
            GroupSelectionChangedCommand = new Util.BuilderCommand(new SelectLastWordAction(this, this).Action);

            Database = DatabaseSingleton.Instance;
            BindingOperations.EnableCollectionSynchronization(Database.Groups, _groupsLock);

            Settings = Settings.GetSettings();
        }

        public override void InitViewModel(object parameter = null)
        {
            SelectedGroup = parameter as IGroup;
            if (SelectedGroup != null)
            {
                SetOnLastWordCurretGroup();
            }
            else
            {
                SetOnLastWord();
            }
        }

        public override void Back()
        {
            UpdateGroup(SelectedGroup);
            UpdateWord(SelectedWord);
        }

        #region Commands

        private void AddClipboardGroup()
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

        private void ChangeLanguage(IGroup group, int selectedLangugage)
        {
            LanguageForGroupProvider provider = new LanguageForGroupProvider
            {
                GroupToChange = group,
                LanguageToChoose = selectedLangugage
            };
            provider.Interact();
        }

        private void ShowWords()
        {
            Switcher.Switch(Switcher.State.Words, SelectedGroup);
        }

        private void FindSame()
        {
            Switcher.Switch(Switcher.State.Same);
        }

        private void SplitGroup(IGroup group)
        {
            if (group == null)
            {
                Console.WriteLine("The parameter is equal null. Cannot perform action.");
                return;
            }
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
            if (!connector.Connect(groups.ToList()) || connector.DestinationGroup == null)
            {
                return;
            }
            foreach (IGroup group in groups.Where(x => x.Words.Count == 0))
            {
                DeleteGroup(group);
            }
            SelectedGroup = connector.DestinationGroup;
        }

        private void RemoveGroup(IGroup group)
        {
            if (group == null)
            {
                Console.WriteLine("The parameter is equal null. Cannot to perform the action.");
                return;
            }
            IInteractionProvider provider = new YesNoProvider()
            {
                ViewModel = new YesNoDialogViewModel()
                {
                    DialogTitle = "Uwaga",
                    Message = "Czy na pewno usunąć grupy?",
                    PositiveLabel = "Tak",
                    NegativeLabel = "Nie",
                    YesAction = new RemoveGroupAction(this, this, DatabaseSingleton.Instance).Action,
                },
            };
            provider.Interact();
        }

        public void TranslateWord()
        {
            SimpleWork work = new SimpleWork();
            work.WorkFunc += SendRequestForWordTranslate;
            BackgroundQueueWithProgressDialog worker = new BackgroundQueueWithProgressDialog();
            ProcessProvider provider = new ProcessProvider()
            {
                ViewModel = new Dialogs.Progress.ProgressDialogViewModel()
                {
                    ButtonLabel = "Anuluj",
                    DialogTitle = "Tłumaczę",
                    CanCanceled = true,
                }
            };
            worker.Dialog = provider;
            worker.AddWork(work);
            worker.Execute();
        }

        private void BackAction()
        {
            UpdateGroup(SelectedGroup);
            UpdateWord(SelectedWord);
            Switcher.Back();
        }

        #endregion

        #region Method

        private void SetOnLastWord()
        {
            SelectedGroup = Database.Groups.LastOrDefault();
            if (SelectedGroup != null)
            {
                SelectedWord = SelectedGroup.Words.LastOrDefault();
            }
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
        }

        private WorkResult SendRequestForWordTranslate()
        {
            IRequest request = new TranslationRequest(new Models.Translator.RequestBag
            {
                From = LanguageFactory.GetLanguage(SelectedGroup.Language1).Code,
                To = LanguageFactory.GetLanguage(SelectedGroup.Language2).Code,
                Word = SelectedWord.Language1
            });
            IConnector<TranslationResponse> connector = new SimpleConnector<TranslationResponse>
            {
                Parser = new TranslationParser()
            };
            TranslationResponse response = null;
            try
            {
                response = connector.SendRequest(request);
            }
            catch (Exception e)
            {
                logger.Error(e);
                return new WorkResult();
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

                ITranslationProvider interactive = new TranslationProvider
                {
                    Items = items,
                    TranslationDirection = TranslationDirection.FromFirst,
                    Word = SelectedWord
                };
                interactive.Interact();
                Language2IsFocused = true;
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
                logger.Error("Błąd Updatu");
            }
        }

        private async void AddWord_(IWord pWord)
        {
            if (!await Database.AddWordAsync(pWord))
            {
                logger.Error("Błąd Dodania");
            }
        }

        private async void AddGroup(IGroup pGroup)
        {
            if (!await Database.AddGroupAsync(pGroup))
            {
                logger.Error("Błąd Dodania");
            }
        }

        private async void UpdateGroup(IGroup pGroup)
        {
            if (!await Database.UpdateGroupAsync(pGroup))
            {
                logger.Error("Błąd Updatu");
            }
        }

        private async void DeleteGroup(IGroup pGroup)
        {
            if (!await Database.DeleteGroupAsync(pGroup))
            {
                logger.Error("Błąd Usuwania");
            }
        }

        public override void Loaded()
        {
        }

        public override void Unloaded()
        {
        }
        #endregion

    }
}
