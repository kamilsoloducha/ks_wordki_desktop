using System.Windows;
using Wordki.Helpers;
using Wordki.Models;

namespace Wordki.ViewModels {
  public abstract class AbstractViewModel {

    private Database Database { get; set; }
    protected bool IsExit;

    protected AbstractViewModel() {
      Database = Database.GetDatabase();
    }

    public void SaveDatabase() {
      if (!Database.User.IsRegister)
        return;
      Database.SaveDatabase();
      CommandQueue<CommandRequest> lQueue = new CommandQueue<CommandRequest>();
      lQueue.MainQueue.Enqueue(new CommandRequest(new WriteGroupsRequest(Database.GetDatabase().User, CsvDatabaseParser.ConvertGroupsToCsv(Database.GetGroupsToSend()))) { OnCompleteCommand = OnWriteGroups });
      lQueue.MainQueue.Enqueue(new CommandRequest(new WriteWordsRequest(Database.GetDatabase().User, CsvDatabaseParser.ConvertWordsToCsv(Database.GetWordsToSend()))) { OnCompleteCommand = OnWriteWords });
      lQueue.MainQueue.Enqueue(new CommandRequest(new WriteResultsRequest(Database.GetDatabase().User, CsvDatabaseParser.ConvertResultsToCsv(Database.GetResultsToSend()))) { OnCompleteCommand = OnWriteResults });
      lQueue.OnQueueComplete += success => {
        if (success) {
          Database.RefreshDatabase();
        } else {

        }
      };
      if (IsExit) {
        lQueue.OnQueueComplete += success => Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
      }
      lQueue.Execute();
    }

    public void LoadDatabase(bool pByDate) {
      if (!Database.User.IsRegister)
        return;
      CommandQueue<CommandRequest> lQueue = new CommandQueue<CommandRequest>();
      lQueue.MainQueue.Enqueue(new CommandRequest(new ReadGroupsRequest(Database.GetDatabase().User)));
      lQueue.MainQueue.Enqueue(new CommandRequest(new ReadWordsRequest(Database.GetDatabase().User)));
      lQueue.MainQueue.Enqueue(new CommandRequest(new ReadResultsRequest(Database.GetDatabase().User)));
      lQueue.OnQueueComplete += success => {
        if (success) {
          Database.RefreshDatabase();
          Database.LoadDatabase();
        } else {

        }
      };
      lQueue.Execute();
    }

    #region UpdateAsyncListener

    protected bool FindError(ResponseEnum lEnum) {
      bool lErrorFound = false;
      switch (lEnum) {
        case ResponseEnum.MESSAGE_ERROR: {
            MessageError();
            lErrorFound = true;
          }
          break;
        case ResponseEnum.CONNECTION_FAILED: {
            ConnectionFailed();
            lErrorFound = true;
          }
          break;
        case ResponseEnum.FILE_ERROR: {
            FileError();
            lErrorFound = true;
          }
          break;
        case ResponseEnum.USER_NOT_FOUND: {
            UserNotFound();
            lErrorFound = true;
          }
          break;
        case ResponseEnum.INTERNET_ERROR: {
            InternetError();
            lErrorFound = true;
          }
          break;
        case ResponseEnum.WRITE_UPDATE_ERROR: {
            UpdateError();
            lErrorFound = true;
          }
          break;
        case ResponseEnum.USER_ALREADY_EXIST: {
            UserAlreadyExist();
            lErrorFound = true;
          }
          break;
        case ResponseEnum.REGISTER_FAILED: {
            RegisterFailed();
            lErrorFound = true;
          }
          break;
      }
      return lErrorFound;
    }

    protected void MessageError() {
      //Parent.ShowToast("Wewnętrzny błąd serwera - błąd wiadomości", ToastLevel.Error);
    }

    protected void InternetError() {
      //Parent.ShowToast("Brak połączenia z internetem", ToastLevel.Error);
    }

    protected void ConnectionFailed() {
      //Parent.ShowToast("Wewnętrzny błąd serwera - błąd połączenia", ToastLevel.Error);
    }

    protected void FileError() {
      //Parent.ShowToast("Wewnętrzny błąd serwera - błąd pliku", ToastLevel.Error);
    }

    protected void UserNotFound() {
      //Parent.ShowToast("Nie znaleziono użytkownika", ToastLevel.Error);
    }

    protected void UpdateError() {
      //Parent.ShowToast("Błąd wysyłania", ToastLevel.Error);
    }

    protected void UserAlreadyExist() {
      //Parent.ShowToast("Istnieje użytkownik o takim loginie", ToastLevel.Error);
    }

    protected void RegisterFailed() {
      //Parent.ShowToast("Wewnętrzny błąd serwera - błąd rejestracji", ToastLevel.Error);
    }

    public void OnWriteGroups(Response pResponse) {
      FindError(pResponse.Enum);
    }

    public void OnWriteWords(Response pResponse) {
      FindError(pResponse.Enum);
    }

    public virtual void OnWriteResults(Response pResponse) {
      FindError(pResponse.Enum);
    }
    //--------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------

    //public void UpdateTranslateListener(Response pResponse) {
    //  Application.Current.Dispatcher.Invoke((Action)(() => {
    //    try {
    //      if (pResponse == null) {
    //        Logger.LogError("{0} - {1}", "BuilderViewModel.UpdateTranslateListener", "pResponse == null");
    //        return;
    //      }
    //      switch (pResponse.Enum) {
    //        case ResponseEnum.FOREIGN_OK: {
    //            RootObject lRootObject = JsonConvert.DeserializeObject<RootObject>(pResponse.Content);
    //            ObservableCollection<string> lTranslationsList = new ObservableCollection<string>();
    //            foreach (var lItem in lRootObject.tuc) {
    //              if (lItem.phrase != null)
    //                lTranslationsList.Add(lItem.phrase.text);
    //            }
    //            ListDialog lDialog = new ListDialog();
    //            lDialog.Items = lTranslationsList;
    //            lDialog.Button1Label = "Wybierz";
    //            lDialog.Button1Command = new BuilderCommand(delegate {
    //              IList lSelectedItems = lDialog.SelectedItems;
    //              StringBuilder lTranslation = new StringBuilder();
    //              for (int i = 0; i < lSelectedItems.Count; i++) {
    //                string lStringItem = lSelectedItems[i] as string;
    //                lTranslation.Append(lStringItem);
    //                if (i < lSelectedItems.Count - 1) {
    //                  lTranslation.Append(", ");
    //                }
    //              }
    //              //if (Native == null || Native.Equals("")) {
    //              //  Native = lTranslation.ToString();
    //              //} else {
    //              //  Foreign = lTranslation.ToString();
    //              //}
    //              lDialog.Close();
    //            });
    //            lDialog.Button2Label = "Anuluj";
    //            lDialog.Button2Command = new BuilderCommand(delegate {
    //              lDialog.Close();
    //            });
    //            lDialog.ShowDialog();
    //          }
    //          break;
    //      }
    //    } catch (Exception lException) {
    //      Logger.LogError("{0} - {1}", "BuilderViewModel.UpdateTranslateListener", lException);
    //    }
    //  }));
    //}

    //public void OnGetCommonGroupsName(Response pResponse) {
    //  Application.Current.Dispatcher.Invoke((Action)(() => {
    //    if (pResponse == null)
    //      return;
    //    if (FindError(pResponse.Enum)) {
    //      return;
    //    }
    //    switch (pResponse.Enum) {
    //      case ResponseEnum.GET_GROUP_NAME_OK:
    //        string[] lElements = pResponse.Content.Split('|');
    //        List<int> lGroupIdList = new List<int>();
    //        List<string> lGroupNameList = new List<string>();
    //        foreach (string lElement in lElements) {
    //          if (!lElement.Contains(";"))
    //            continue;
    //          string[] lItems = lElement.Split(';');
    //          lGroupIdList.Add(Int32.Parse(lItems[0]));
    //          lGroupNameList.Add(lItems[1]);
    //        }
    //        ListDialog lListDialog = new ListDialog();
    //        lListDialog.Button1Label = "Pobierz";
    //        lListDialog.Button2Label = "Anuluj";
    //        lListDialog.Items = new ObservableCollection<string>(lGroupNameList);
    //        lListDialog.Button1Command = new BuilderCommand(delegate {
    //          if (lListDialog.SelectedItem == null)
    //            return;
    //          GroupToDownload = lListDialog.SelectedItem;
    //          Request lRequest = new GetCommonGroupRequest(Database.GetDatabase().User, lGroupIdList[lListDialog.SelectedIndex]);
    //          //SendRequest(lRequest, "Pobieranie", "Pobieram grupę z serwera", OnGetCommonGroups);
    //          lListDialog.Close();
    //        });
    //        lListDialog.Button2Command = new BuilderCommand(delegate {
    //          lListDialog.Close();
    //        });
    //        lListDialog.ShowDialog();
    //        break;
    //    }
    //  }));
    //}

    //public virtual void OnGetCommonGroups(Response pResponse) {
    //  Application.Current.Dispatcher.Invoke((() => {
    //    if (pResponse == null)
    //      return;
    //    if (FindError(pResponse.Enum)) {
    //      return;
    //    }
    //    switch (pResponse.Enum) {
    //      case ResponseEnum.GET_GROUP_OK:
    //        List<Word> lWordsList = CsvDatabaseParser.ConvertCsvToWords(pResponse.Content);
    //        Group lNewGroup = new Group();
    //        lNewGroup.GroupName = GroupToDownload;
    //        if (!Database.GetDatabase().AddGroup(lNewGroup)) {
    //          Logger.LogError("{0} - {1}", "Blad w czasie dodawania grupy", "lNewGroup == null");
    //          return;
    //        }
    //        foreach (Word lWord in lWordsList) {
    //          lWord.State = ElementState.Update;
    //          Database.GetDatabase().AddWord(lNewGroup, lWord);
    //        }
    //        //Parent.ShowToast("Pobrano grupę", ToastLevel.Info);
    //        break;
    //    }
    //  }));
    //}
    #endregion
  }
}
