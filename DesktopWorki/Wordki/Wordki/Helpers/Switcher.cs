using System;
using System.Collections.Generic;
using Wordki.Views;

namespace Wordki.Helpers {
  public class Switcher {

    private static Switcher _instance;

    public static Switcher GetSwitcher() {
      if (_instance == null) {
        _instance = new Switcher();
      }
      return _instance;
    }

    private Switcher() {
      LockStates = new List<State>();
    }


    public List<State> LockStates { get; private set; }
    private readonly Stack<State> _pageQueue = new Stack<State>();
    private readonly Dictionary<State, ISwitchElement> _pageDictionary = new Dictionary<State, ISwitchElement>();

    public void Switch(State pState) {
      _pageQueue.Push(pState);
      ISwitchElement lPage;
      if (_pageDictionary.ContainsKey(pState)) {
        lPage = _pageDictionary[pState];
      } else {
        lPage = PageFactory(pState);
        _pageDictionary.Add(pState, lPage);
      }
      if (OnSwich != null) {
        OnSwich.Invoke(this, new SwitchEventArgs(lPage));
      }
    }

    private ISwitchElement PageFactory(State pState) {
      switch (pState) {
        case State.Login:
          return new LoginPage();
        case State.Register:
          return new RegisterPage();
        case State.Menu:
          return new MenuPage();
        case State.Builder:
          return new BuilderPage();
        case State.Groups:
          return new GroupsPage();
        case State.Teach:
          return new TeachPage();
        case State.Settings:
          return new SettingsPage();
        case State.Words:
          return new WordsPage();
        case State.Plot:
          return new PlotPage();
        case State.Same:
          return new SameWordsPage();
        case State.BuildFromFile:
          return new BuildFromFilePage();
        default:
          throw new ArgumentOutOfRangeException("pState");
      }
    }

    public void Back(bool pForce = false) {
      if (!pForce && LockStates.Contains(_pageQueue.Peek())) {
        return;
      }
      if (_pageQueue.Count == 1) {
        Reset();
        return;
      }
      _pageDictionary[_pageQueue.Pop()].ViewModel.Back();
      Switch(_pageQueue.Pop());
    }

    public void Reset() {
      _pageQueue.Clear();
      _pageDictionary.Clear();
      Switch(State.Login);
    }

    public enum State {
      Login,
      Register,
      Menu,
      Builder,
      Groups,
      Teach,
      Settings,
      Words,
      Plot,
      Same,
      BuildFromFile,
    }

    public event EventHandler OnSwich;

    public class SwitchEventArgs : EventArgs {

      public ISwitchElement Page { get; private set; }

      public SwitchEventArgs(ISwitchElement pPage) {
        Page = pPage;
      }
    }
  }

  public interface ISwitchElement{
    IViewModel ViewModel { get; }
  }

  public interface IViewModel {
    void InitViewModel();
    void Back();
  }

}
