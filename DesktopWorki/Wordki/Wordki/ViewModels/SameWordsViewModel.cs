using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Wordki.Helpers;
using Wordki.Models;
using Wordki.Views.Dialogs;

namespace Wordki.ViewModels {
  public class SameWordsViewModel : INotifyPropertyChanged, IViewModel {
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string propertyName) {
      PropertyChangedEventHandler handler = PropertyChanged;

      if (handler != null) {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }
    #endregion

    private Database Database { get; set; }
    public ObservableCollection<Word> DataGridCollection { get; set; }
    public BuilderCommand BackCommand { get; set; }
    public BuilderCommand ConnectWordsCommand { get; set; }
    public BuilderCommand EditWordCommand { get; set; }
    public BuilderCommand BothLanguagesCommand { get; set; }

    private object _lockObject = new object();

    public SameWordsViewModel() {
      ActivateCommand();
      DataGridCollection = new ObservableCollection<Word>();
    }

    private void ActivateCommand() {
      BackCommand = new BuilderCommand(Back);
      ConnectWordsCommand = new BuilderCommand(ConnectWord);
      EditWordCommand = new BuilderCommand(EditWord);
      BothLanguagesCommand = new BuilderCommand(BothLanguages);
    }

    private void EditWord(object obj) {
      try {
        if (obj == null) {
          return;
        }
        Word lWordItem = obj as Word;
        if (lWordItem == null) {
          return;
        }
        Group lGroup = Database.GroupsList.FirstOrDefault(x => x.Id == lWordItem.GroupId);
        if (lGroup == null)
          return;
        Word lSelectedWord = lGroup.WordsList.FirstOrDefault(x => x.Id == lWordItem.Id);
        if (lSelectedWord == null)
          return;

        CorrectWordDialog lDialog = new CorrectWordDialog(lSelectedWord);
        lDialog.ShowDialog();
      } catch (Exception lException) {
        Logger.LogError("{0} - {1}", "SameWordsViewModel.EditWord", lException.Message);
      }
    }

    private void BothLanguages(object obj) {
      CreateDataGridCollection();
    }

    public void InitViewModel() {
      Database = Database.GetDatabase();
    }

    public void Back() {
      
    }


    private async void ConnectWord(object obj) {
      if (obj == null)
        return;
      IList lList = (obj as IList);
      if (lList == null)
        return;
      List<Word> lSameWordListSelected = lList.Cast<Word>().ToList();
      if (lSameWordListSelected.Count < 2) {
        return;
      }
      Word lSameWordDataGridItem = lSameWordListSelected[0];
      StringBuilder lLanguage1 = new StringBuilder();
      StringBuilder lLanguage2 = new StringBuilder();
      StringBuilder lLanguage1Comment = new StringBuilder();
      StringBuilder lLanguage2Comment = new StringBuilder();
      foreach (Word lItem in lSameWordListSelected) {
        if (!lLanguage1.ToString().Contains(lItem.Language1)) {
          lLanguage1.Append(lItem.Language1);
          lLanguage1.Append(", ");
        }
        if (!lLanguage2.ToString().Contains(lItem.Language2)) {
          lLanguage2.Append(lItem.Language2);
          lLanguage2.Append(", ");
        }
        if (!lLanguage1Comment.ToString().Contains(lItem.Language1Comment)) {
          lLanguage1Comment.Append(lItem.Language1Comment);
          lLanguage1Comment.Append(". ");
        }
        if (!lLanguage2Comment.ToString().Contains(lItem.Language2Comment)) {
          lLanguage2Comment.Append(lItem.Language2Comment);
          lLanguage2Comment.Append(". ");
        }
      }
      lLanguage1.Remove(lLanguage1.Length - 2, 2);
      lLanguage2.Remove(lLanguage2.Length - 2, 2);

      lSameWordDataGridItem.Language1 = lLanguage1.ToString();
      lSameWordDataGridItem.Language2 = lLanguage2.ToString();
      lSameWordDataGridItem.Language1Comment = lLanguage1Comment.ToString();
      lSameWordDataGridItem.Language2Comment = lLanguage2Comment.ToString();
      lSameWordDataGridItem.Visible = true;
      lSameWordDataGridItem.Drawer = 0;
      await Database.UpdateWordAsync(lSameWordDataGridItem);
      for (int i = 1; i < lSameWordListSelected.Count; i++) {
        lSameWordListSelected[i].State = -1;
        await Database.UpdateWordAsync(lSameWordListSelected[i]);
      }
    }

    private void Back(object obj) {
      Switcher.GetSwitcher().Back();
    }

    private async void CreateDataGridCollection() {
      BindingOperations.EnableCollectionSynchronization(Database.GroupsList, _lockObject);
      await Task.Run(() => {
        DataGridCollection.Clear();
        List<Word> lSameWordsList = FindSameWords();
        foreach (Word lWord in lSameWordsList) {
          Group lGroup = Database.GroupsList.FirstOrDefault(x => x.Id == lWord.GroupId);
          if (lGroup == null)
            continue;
          DataGridCollection.Add(lWord);
        }
      });
      BindingOperations.DisableCollectionSynchronization(Database.GroupsList);
    }

    private List<Word> FindSameWords() {
      List<Word> lSameWordsList = new List<Word>();
      List<Word> lAllWords = new List<Word>();
      try {
        foreach (Group lGroup in Database.GroupsList) {
          lAllWords.AddRange(lGroup.WordsList);
        }
        int lIsSame;
        foreach (Word lWord1 in lAllWords) {
          foreach (Word lWord2 in lAllWords) {
            lIsSame = 0;
            if (lWord1.Id == lWord2.Id)
              continue;
            if (lIsSame == 0 && lWord1.Language1.Equals(lWord2.Language1))
              lIsSame++;
            if (lIsSame == 0 && lWord1.Language2.Equals(lWord2.Language2))
              lIsSame++;
            if (lIsSame == 0 && IsPartialSame(lWord1.Language1, lWord2.Language1))
              lIsSame++;
            if (lIsSame == 0 && IsPartialSame(lWord1.Language2, lWord2.Language2))
              lIsSame++;

            if (lIsSame <= 0) continue;
            if (lSameWordsList.FirstOrDefault(x => x.Id == lWord2.Id) != null)
              continue;
            lSameWordsList.Add(lWord2);
            lSameWordsList.Add(lWord1);
          }
        }
      } catch (Exception lException) {
        Logger.LogError("{0} - {1}", "BuilderViewModel.FindSame", lException.Message);
      }
      return lSameWordsList;
    }

    private bool IsPartialSame(string lWord1, string lWord2) {
      try {
        string[] lWord1Array = lWord1.Split(' ');
        string[] lWord2Array = lWord2.Split(' ');
        if (lWord1Array.Count() == 1 || lWord2Array.Count() == 1) {
          return false;
        }
        for (int i = 0; i < lWord1Array.Count(); i++) {
          lWord1Array[i] = lWord1Array[i].Trim(',', '.', '-', '\\', '/');
        }
        for (int i = 0; i < lWord2Array.Count(); i++) {
          lWord2Array[i] = lWord2Array[i].Trim(',', '.', '-', '\\', '/');
        }
        if (lWord1Array.Where(lItem1 => lItem1.Length >= 4).Any(lItem1 => lWord2Array.Where(lItem2 => lItem2.Length >= 4).Any(lItem1.Equals))) {
          return true;
        }
      } catch (Exception lException) {
        Logger.LogError("{0} - {1}", "BuilderViewModel.IsPartialSame", lException.Message);
      }
      return false;
    }


    
  }
}
