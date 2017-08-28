using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Wordki.ViewModels {
  public enum EnableElementBuilder {
    Previous,
    Next,
    GroupName,
    Language1,
    Language2,
    Language1Comment,
    Language2Comment,
  }

  public class BuilderViewModel : INotifyPropertyChanged, IViewModel {

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
    #endregion

    private readonly object _groupsLock = new object();
    private string _nextGroupLabel;
    private string _previousGroupLabel;
    private bool _languages1IsFocused;
    private Group _selectedGroup;
    private Word _selectedWord;

    #region Properies

    public Group SelectedGroup {
      get { return _selectedGroup; }
      set {
        if (_selectedGroup != null && _selectedGroup.Equals(value))
          return;
        UpdateGroup(_selectedGroup);
        _selectedGroup = value;
        OnPropertyChanged();
      }
    }
    public Word SelectedWord {
      get { return _selectedWord; }
      set {
        if (_selectedWord != null && _selectedWord.Equals(value))
          return;
        UpdateWord(_selectedWord);
        _selectedWord = value;
        OnPropertyChanged();
      }
    }
    public string NextGroupLabel {
      get { return _nextGroupLabel; }
      set {
        if (_nextGroupLabel == value)
          return;
        _nextGroupLabel = value;
        OnPropertyChanged();
      }
    }
    public string PreviousGroupLabel {
      get { return _previousGroupLabel; }
      set {
        if (_previousGroupLabel == value)
          return;
        _previousGroupLabel = value;
        OnPropertyChanged();
      }
    }
    public bool Language1IsFocused {
      get { return _languages1IsFocused; }
      set {
        if (_languages1IsFocused != value) {
          _languages1IsFocused = value;
          OnPropertyChanged();
        }
      }
    }

    private int _groupCount;
    public int GroupCount {
      get { return _groupCount; }
      set {
        if (_groupCount == value)
          return;
        _groupCount = value;
        OnPropertyChanged();
      }
    }

    private int? _groupNumber;
    public int? GroupNumber {
      get { return _groupNumber; }
      set {
        if (_groupNumber == value)
          return;
        _groupNumber = value;
        OnPropertyChanged();
      }
    }

    private int? _drawer;
    public int? Drawer {
      get { return _drawer; }
      set {
        if (_drawer == value)
          return;
        _drawer = value;
        OnPropertyChanged();
      }
    }

    private int? _wordNumber;
    public int? WordNumber {
      get { return _wordNumber; }
      set {
        if (_wordNumber == value)
          return;
        _wordNumber = value;
        OnPropertyChanged();
      }
    }


    public BuilderCommand PreviousWordCommand { get; set; }
    public BuilderCommand NextWordCommand { get; set; }
    public BuilderCommand PreviousGroupCommand { get; set; }
    public BuilderCommand NextGroupCommand { get; set; }
    public BuilderCommand TranslateWordCommand { get; set; }
    public BuilderCommand AddWordCommand { get; set; }
    public BuilderCommand RemoveWordCommand { get; set; }

    public BuilderCommand BackCommand { get; set; }

    public BuilderCommand DownloadGroupsNameCommand { get; set; }
    public BuilderCommand AddGroupCommand { get; set; }
    public BuilderCommand AddClipboardGroupCommand { get; set; }
    public BuilderCommand RemoveGroupCommand { get; set; }
    public BuilderCommand SplitGroupCommand { get; set; }
    public BuilderCommand ConnectGroupCommand { get; set; }

    public BuilderCommand FindSameWordCommand { get; set; }
    public BuilderCommand ShowWordsCommnad { get; set; }

    public BuilderCommand SwapLanguagesCommand { get; set; }
    public BuilderCommand SwapSingleWordCommand { get; set; }

    public BuilderCommand ChangeLanguage1Command { get; set; }
    public BuilderCommand ChangeLanguage2Command { get; set; }

    public BuilderCommand WordSelectionChangedCommand { get; set; }
    public BuilderCommand GroupSelectionChangedCommand { get; set; }
    public BuilderCommand AddGroupFromFileCommand { get; set; }

    public Database Database { get; set; }
    public ObservableDictionary<string, bool> EnableElementDirectory { get; set; }
    //private ClipboardHelper ClipboardHelper { get; set; }
    //private NotifyIcon NotifyIcon { get; set; }
    public Settings Settings { get; set; }

    #endregion

    public BuilderViewModel() {
      ActivateCommands();
      Database = Database.GetDatabase();
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

    public void InitViewModel() {
      Group lGroup = PackageStore.Get(0) as Group;
      if (lGroup != null) {
        SelectedGroup = lGroup;
        SetOnLastWordCurretGroup();
      } else {
        SetOnLastWord();
      }
      RefreshView();
    }

    public void Back() {
      Database.SaveDatabase();
      var queue = RemoteDatabaseAbs.GetRemoteDatabase(Database.User).GetUploadQueue();
      queue.CreateDialog = false;
      queue.Execute();
    }

    private async void UpdateWord(Word pWord) {
      if (!await Database.UpdateWordAsync(pWord)) {
        Logger.LogError("Błąd Updatu");
      }
    }

    private async void AddWord(Group pGroup, Word pWord) {
      if (!await Database.AddWordAsync(pGroup, pWord)) {
        Logger.LogError("Błąd dodanie słowa do bazy");
      }
    }

    private async void AddGroup(Group pGroup) {
      if (!await Database.AddGroupAsync(pGroup)) {
        Logger.LogError("Błąd Dodania");
      }
    }

    private async void UpdateGroup(Group pGroup) {
      if (!await Database.UpdateGroupAsync(pGroup)) {
        Logger.LogError("Błąd Updatu");
      }
    }

    private async void DeleteGroup(Group pGroup) {
      if (!await Database.DeleteGroupAsync(pGroup)) {
        Logger.LogError("Błąd Usuwania");
      }
    }

    #region Commands
    private void ActivateCommands() {
      PreviousWordCommand = new BuilderCommand(PreviousWord);
      NextWordCommand = new BuilderCommand(NextWord);
      PreviousGroupCommand = new BuilderCommand(PreviuosGroup);
      NextGroupCommand = new BuilderCommand(NextGroup);

      TranslateWordCommand = new BuilderCommand(Translate);

      AddWordCommand = new BuilderCommand(AddWord);
      RemoveWordCommand = new BuilderCommand(DeleteWord);
      AddGroupCommand = new BuilderCommand(AddGroup);
      RemoveGroupCommand = new BuilderCommand(RemoveGroup);

      DownloadGroupsNameCommand = new BuilderCommand(DownloadGroupsName);
      BackCommand = new BuilderCommand(Back);

      SplitGroupCommand = new BuilderCommand(SplitGroup);
      ConnectGroupCommand = new BuilderCommand(ConnectGroup);

      FindSameWordCommand = new BuilderCommand(FindSame);
      ShowWordsCommnad = new BuilderCommand(ShowWords);
      SwapLanguagesCommand = new BuilderCommand(SwapWords);
      SwapSingleWordCommand = new BuilderCommand(SwapSingleWord);

      ChangeLanguage1Command = new BuilderCommand(ChangeLanguage1);
      ChangeLanguage2Command = new BuilderCommand(ChangeLanguage2);
      AddClipboardGroupCommand = new BuilderCommand(AddClipboardGroup);
      WordSelectionChangedCommand = new BuilderCommand(WordSelectionChanged);
      GroupSelectionChangedCommand = new BuilderCommand(GroupSelectionChanged);
      AddGroupFromFileCommand = new BuilderCommand(AddGroupFromFile);
    }

    private void AddGroupFromFile(object obj) {
      Switcher.GetSwitcher().Switch(Switcher.State.BuildFromFile);
    }

    private void GroupSelectionChanged(object obj) {
      SetOnLastWordCurretGroup();
      RefreshView();
    }

    private void WordSelectionChanged(object obj) {
      RefreshView();
    }

    private void AddClipboardGroup(object obj) {
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

    private void SwapWords(object obj) {
      if (Database.GroupsList.Count == 0)
        return;
      Group lGroup = SelectedGroup;
      lGroup.SwapLanguage();
      Database.UpdateGroupAsync(lGroup).ConfigureAwait(false);
      foreach (Word lWord in lGroup.WordsList) {
        lWord.SwapLanguages();
        Database.UpdateWordAsync(lWord).ConfigureAwait(false);
      }
      foreach (Result result in lGroup.ResultsList) {
        result.SwapDirection();
        Database.UpdateResultAsync(result).ConfigureAwait(false);
      }
      SetWordLabels();
    }

    private void SwapSingleWord(object obj) {
      if (SelectedWord == null) {
        return;
      }
      SelectedWord.SwapLanguages();
      Database.UpdateWordAsync(SelectedWord).ConfigureAwait(false);
    }

    private void ChangeLanguage2(object obj) {
      SelectLangauge(2);
    }

    private void ChangeLanguage1(object obj) {
      SelectLangauge(1);
    }

    private void ShowWords(object obj) {
      if (Database.GroupsList.Count == 0)
        return;
      long lGroupId = SelectedGroup.Id;
      PackageStore.Put(0, lGroupId);
      Switcher.GetSwitcher().Switch(Switcher.State.Words);
    }

    private void FindSame(object obj) {
      Switcher.GetSwitcher().Switch(Switcher.State.Same);
    }

    private void SplitGroup(object obj) {
      if (Database.GroupsList.Count == 0)
        return;
      SplitGroupDialog lDialog = new SplitGroupDialog();
      lDialog.ViewModel.OnOkClickListener += () => {
        GroupSplitterBase lSpliter;
        switch (lDialog.ViewModel.TabSelected) {
          case (int)SplitterEnum.Percentage: {
              lSpliter = new GroupSlitPercentage(lDialog.ViewModel.Percentage, SelectedGroup, Database);
            }
            break;
          case (int)SplitterEnum.GroupCount: {
              lSpliter = new GroupSplitGroupCount(Int32.Parse(lDialog.ViewModel.GroupCount), SelectedGroup, Database);
            }
            break;
          case (int)SplitterEnum.WordCount: {
              lSpliter = new GroupSplitWordCount(Int32.Parse(lDialog.ViewModel.WordCount), SelectedGroup, Database);
            }
            break;
          default:
            return;
        }
        List<Group> lGroups = lSpliter.Split();
        if (lGroups == null)
          return;
        foreach (Group lGroup in lGroups) {
          AddGroup(lGroup);
        }
        Task.Run(() => {
          Database.SaveDatabase();
          Application.Current.Dispatcher.Invoke(SetOnLastWord);
        });
      };
      lDialog.ShowDialog();
    }

    private void ConnectGroup(object obj) {
      try {

      } catch (Exception lException) {
        Logger.LogError("{0} - {1}", "BuilderViewModel.ConnectGroup", lException.Message);
      }
    }

    private void RemoveGroup(object obj) {
      if (SelectedGroup == null) {
        return;
      }
      YesNoDialog dialog = new YesNoDialog {
        DialogTitle = "Uwaga",
        Message = "Czy na pewno usunąć grupy?",
        PositiveLabel = "Tak",
        NegativeLabel = "Nie",
      };
      dialog.PositiveCommand = new BuilderCommand(o => {
        Group groupToDelete = SelectedGroup;
        int groupIndex = Database.GroupsList.IndexOf(SelectedGroup);
        SelectedGroup = Database.GroupsList.Count > groupIndex ? Database.GroupsList[groupIndex] : null;
        SelectedWord = SelectedGroup != null ? SelectedGroup.WordsList.LastOrDefault() : null;
        DeleteGroup(groupToDelete);
        dialog.Close();
        RefreshView();
      });
      dialog.NegativeCommand = new BuilderCommand(o => dialog.Close());
      dialog.ShowDialog();
    }

    private void AddGroup(object obj) {
      Group lNewGroup = new Group();
      AddGroup(lNewGroup);
      SelectedGroup = lNewGroup;
      SetOnLastWordCurretGroup();
      RefreshView();
    }

    private void DownloadGroupsName(object obj) {
    }

    private string GroupToDownload { get; set; }
    private void OnGetCommonGroupNameRequestComplete() {
      
    }

    private void NextWord(object obj) {
      if (SelectedWord == null) {
        return;
      }
      int wordIndex = SelectedGroup.WordsList.IndexOf(SelectedWord);
      if (wordIndex < SelectedGroup.WordsList.Count - 1) {
        SelectedWord = SelectedGroup.WordsList[++wordIndex];
        RefreshView();
      }
    }

    private void PreviousWord(object obj) {
      if (SelectedWord == null) {
        return;
      }
      int wordIndex = SelectedGroup.WordsList.IndexOf(SelectedWord);
      if (wordIndex > 0) {
        SelectedWord = SelectedGroup.WordsList[--wordIndex];
        RefreshView();
      }
    }

    private void PreviuosGroup(object obj) {
      int lGroupIndex = Database.GroupsList.IndexOf(SelectedGroup);
      if (lGroupIndex > 0) {
        SelectedGroup = Database.GroupsList[--lGroupIndex];
        SetOnLastWordCurretGroup();
        RefreshView();
      }
    }

    private void NextGroup(object obj) {
      int lGroupIndex = Database.GroupsList.IndexOf(SelectedGroup);
      if (lGroupIndex < Database.GroupsList.Count - 1) {
        SelectedGroup = Database.GroupsList[++lGroupIndex];
        SetOnLastWordCurretGroup();
      }
    }

    private void Back(object obj) {
      Switcher.GetSwitcher().Back();
    }

    private void Translate(object obj) {
    }

    private void AddWord(object obj) {
      if (SelectedGroup == null) {
        AddGroupCommand.Execute(null);
      }
      if (SelectedGroup == null) {
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


    private async void DeleteWord(object obj) {
      //nie ma grup to wychodzi
      if (SelectedWord == null && SelectedGroup != null) {
        RemoveGroup(null);
        RefreshView();
        return;
      }
      if (await Database.DeleteWordAsync(SelectedGroup, SelectedWord)) {
        SelectedWord = SelectedGroup.WordsList.LastOrDefault();
        if (SelectedWord == null) {
          RemoveGroup(null);
        }
      }
      RefreshView();
    }

    private void SelectLangauge(int pLanguageIndex) {
      if (Database.GroupsList.Count == 0)
        return;
      LanguageListDialog lDialog = new LanguageListDialog {
        Items = LanguageFactory.GetLanguages(),
        Button1Label = "Wybierz"
      };
      lDialog.Button1Command = new BuilderCommand(delegate {
        Group lGroup = SelectedGroup;
        LanguageType lSelectedLanguage = (LanguageType)lDialog.SelectedIndex;
        if (pLanguageIndex == 1) {
          lGroup.Language1 = LanguageFactory.GetLanguage(lSelectedLanguage);
        } else {
          lGroup.Language2 = LanguageFactory.GetLanguage(lSelectedLanguage);
        }
        lDialog.Close();
      });
      lDialog.Button2Label = "Anuluj";
      lDialog.Button2Command = new BuilderCommand(delegate {
        lDialog.Close();
      });
      lDialog.ShowDialog();
    }
    #endregion

    #region Method

    public void RefreshView() {
      SetGroupNameLabel();
      SetWordLabels();
      SetNextGroupButton();
      SetPreviousGroupButton();
      SetInfo();
    }

    private void SetGroupNameLabel() {
      if (SelectedGroup == null) {
        UnableGroupName();
      } else {
        EnableGroupName();
      }
    }

    private void UnableGroupName() {
      EnableElementDirectory[EnableElementBuilder.GroupName.ToString()] = false;
    }

    private void EnableGroupName() {
      EnableElementDirectory[EnableElementBuilder.GroupName.ToString()] = true;
    }

    private void SetWordLabels() {
      if (SelectedWord == null) {
        UnableWord();
      } else {
        EnableWord();
      }
    }

    private void EnableWord() {
      EnableElementDirectory[EnableElementBuilder.Language1.ToString()] = true;
      EnableElementDirectory[EnableElementBuilder.Language2.ToString()] = true;
      EnableElementDirectory[EnableElementBuilder.Language1Comment.ToString()] = true;
      EnableElementDirectory[EnableElementBuilder.Language2Comment.ToString()] = true;
    }

    private void UnableWord() {
      EnableElementDirectory[EnableElementBuilder.Language1.ToString()] = false;
      EnableElementDirectory[EnableElementBuilder.Language2.ToString()] = false;
      EnableElementDirectory[EnableElementBuilder.Language1Comment.ToString()] = false;
      EnableElementDirectory[EnableElementBuilder.Language2Comment.ToString()] = false;
    }

    private void SetNextGroupButton() {
      if (SelectedGroup != null) {
        int lGroupIndex = Database.GroupsList.IndexOf(SelectedGroup);
        if (lGroupIndex < Database.GroupsList.Count - 1) {
          NextGroupLabel = Database.GroupsList[lGroupIndex + 1].Name;
          EnableElementDirectory[EnableElementBuilder.Next.ToString()] = true;
        } else {
          NextGroupLabel = "";
          EnableElementDirectory[EnableElementBuilder.Next.ToString()] = false;
        }
      } else {
        NextGroupLabel = "";
        EnableElementDirectory[EnableElementBuilder.Next.ToString()] = false;
      }
    }

    private void SetPreviousGroupButton() {
      if (SelectedGroup != null) {
        int lGroupIndex = Database.GroupsList.IndexOf(SelectedGroup);
        if (lGroupIndex > 0) {
          PreviousGroupLabel = Database.GroupsList[lGroupIndex - 1].Name;
          EnableElementDirectory[EnableElementBuilder.Previous.ToString()] = true;
        } else {
          PreviousGroupLabel = "";
          EnableElementDirectory[EnableElementBuilder.Previous.ToString()] = false;
        }
      } else {
        PreviousGroupLabel = "";
        EnableElementDirectory[EnableElementBuilder.Previous.ToString()] = false;
      }
    }

    private void SetInfo() {
      GroupCount = Database.GroupsList.Count;
      if (SelectedGroup != null) {
        GroupNumber = Database.GroupsList.IndexOf(SelectedGroup) + 1;
      } else {
        GroupNumber = null;
      }
      if (SelectedGroup != null && SelectedWord != null) {
        WordNumber = SelectedGroup.WordsList.IndexOf(SelectedWord) + 1;
        Drawer = SelectedWord.Drawer + 1;
      } else {
        WordNumber = null;
        Drawer = null;
      }
    }

    private void SetOnLastWord() {
      SelectedGroup = Database.GroupsList.LastOrDefault();
      if (SelectedGroup == null) {
        SelectedWord = null;
      } else {
        //RefreshWordsList();
        SelectedWord = SelectedGroup.WordsList.LastOrDefault();
      }
      RefreshView();
    }

    private void SetOnLastWordCurretGroup() {
      if (SelectedGroup == null) {
        SelectedWord = null;
      } else {
        SelectedWord = SelectedGroup.WordsList.LastOrDefault();
      }
      RefreshView();
    }
    #endregion

  }
}
