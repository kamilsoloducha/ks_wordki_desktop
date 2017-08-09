using System;
using System.Collections.Generic;
using System.Linq;
using Wordki.Helpers;
using Wordki.Models.Lesson.WordComparer;

namespace Wordki.Models.Lesson {

  [Serializable]
  public abstract class Lesson {
    public List<Result> ResultList { get; protected set; }
    public Queue<Word> WordList { get; protected set; }
    public IList<Word> BeginWordsList { get; protected set; }
    public Word SelectedWord { get; protected set; }
    public string Translation { get; set; }
    public bool IsCorrect { get; protected set; }
    public bool IsChecked { get; protected set; }
    public int CurrentDrawer { get; protected set; }
    public Timer Timer { get; private set; }
    public int Counter { get; set; }
    public IWordComparer WordComparer { get; set; }

    protected Lesson() {
      BeginWordsList = new List<Word>();
      WordList = new Queue<Word>();
      Timer = new Timer();
      Counter = 1;
    }

    protected abstract void CreateWordList();
    protected abstract void CreateResultList();
    public abstract void Known();
    public abstract void Check();

    public virtual void InitLesson() {
      CreateWordList();
      CreateResultList();
    }

    /// <summary>
    /// Wybranie kolejnego slowa
    /// </summary>
    public virtual void NextWord() {
      SelectedWord = (WordList.Count > 0) ? WordList.Dequeue() : null;
      IsChecked = false;
      if (SelectedWord != null)
        CurrentDrawer = SelectedWord.Drawer;
    }

    /// <summary>
    /// Zaznaczenie slowa jako nieznane
    /// </summary>
    public virtual void Unknown() {
      if (Counter++ <= BeginWordsList.Count) {
        SelectedWord.Drawer = 0; //reset szuflady
        Result lResult = ResultList.FirstOrDefault(x => x.GroupId == SelectedWord.GroupId);
        if (lResult != null) {
          lResult.Wrong++;
        }
      }
      //przesuwamy SelectedWord na poczatek listy
      WordList.Enqueue(SelectedWord);
      NextWord();
    }

    protected virtual bool CheckTranslation(string pOriginalWord, string pTranslationWord) {
      return WordComparer.Compare(pOriginalWord, pTranslationWord);
    }

    public virtual void FinishLesson() {
      foreach (Result lResult in ResultList) {
        lResult.TimeCount = (short)(Timer.GetTime() / ResultList.Count);
      }
    }

    public int GetCorrect() {
      return ResultList.Sum(lResult => lResult.Correct);
    }

    public int GetAccepted() {
      return ResultList.Sum(lResult => lResult.Accepted);
    }

    public int GetWrong() {
      return ResultList.Sum(lResult => lResult.Wrong);
    }

    public virtual int GetMaxResult() {
      return GetWrong() + GetAccepted() + GetCorrect();
    }

    public virtual List<int> GetDrawerCount() {
      List<int> lDrawerCountList = new List<int>();
      for (int i = 0; i < 5; i++) {
        lDrawerCountList.Add(0);
      }
      foreach (Word lWord in BeginWordsList) {
        lDrawerCountList[lWord.Drawer]++;
      }
      return lDrawerCountList;
    }

    public void DeleteSelectedWord() {
      BeginWordsList.Remove(SelectedWord);
    }

    public virtual int[] GetDrawerValues() {
      int[] lTempValues = new int[5];
      foreach (Word lWord in BeginWordsList) {
        lTempValues[lWord.Drawer]++;
      }
      return lTempValues;
    }

    public virtual double GetProgress() {
      int remain = WordList.Count + (SelectedWord == null ? 0 : 1);
      return (BeginWordsList.Count - remain) * 100d / BeginWordsList.Count;
    }
  }
}
