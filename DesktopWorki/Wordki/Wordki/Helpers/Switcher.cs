using System;
using System.Collections.Generic;
using Wordki.ViewModels;
using Wordki.Views;

namespace Wordki.Helpers
{
    public class Switcher
    {
        public Switcher()
        {
            LockStates = new List<State>();
        }

        public List<State> LockStates { get; private set; }
        private readonly Stack<State> _pageQueue = new Stack<State>();
        private readonly Dictionary<State, ISwitchElement> _pageDictionary = new Dictionary<State, ISwitchElement>();

        public void Switch(State pState)
        {
            _pageQueue.Push(pState);
            ISwitchElement lPage;
            if (_pageDictionary.ContainsKey(pState))
            {
                lPage = _pageDictionary[pState];
                _pageDictionary.Remove(pState);
            }
            else
            {
                lPage = PageFactory(pState);
                _pageDictionary.Add(pState, lPage);
            }
            if (OnSwich != null)
            {
                OnSwich.Invoke(this, new SwitchEventArgs(lPage));
            }
        }

        private ISwitchElement PageFactory(State pState)
        {
            ISwitchElement result;
            switch (pState)
            {
                case State.Login:
                    result = new LoginPage();
                    break;
                case State.Register:
                    result = new RegisterPage();
                    break;
                case State.Menu:
                    result = new MenuPage();
                    break;
                case State.Builder:
                    result = new BuilderPage();
                    break;
                case State.Groups:
                    result = new GroupsPage();
                    break;
                case State.TeachTyping:
                    result = new TeachPage(TeachPageType.Typing);
                    break;
                case State.TeachFiszki:
                    result = new TeachPage(TeachPageType.Fiszki);
                    break;
                case State.Settings:
                    result = new SettingsPage();
                    break;
                case State.Words:
                    result = new WordsPage();
                    break;
                //case State.Plot:
                //    result = new PlotPage();
                    //break;
                case State.Same:
                    result = new SameWordsPage();
                    break;
                case State.BuildFromFile:
                    result = new BuildFromFilePage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("pState");
            }
            result.ViewModel.Switcher = this;
            return result;
        }

        public void Back(bool pForce = false)
        {
            if (!pForce && LockStates.Contains(_pageQueue.Peek()))
            {
                return;
            }
            if (_pageQueue.Count == 1)
            {
                Reset();
                return;
            }
            _pageDictionary[_pageQueue.Pop()].ViewModel.Back();
            Switch(_pageQueue.Pop());
        }

        public void Reset()
        {
            _pageQueue.Clear();
            _pageDictionary.Clear();
            Switch(State.Login);
        }

        public void LoadStates()
        {
            //foreach(State state in Enum.GetValues(typeof(State)))
            //{
            //    Switch(state);
            //}
        }

        public enum State
        {
            Login,
            Register,
            Menu,
            Builder,
            Groups,
            TeachTyping,
            TeachFiszki,
            Settings,
            Words,
            //Plot,
            Same,
            BuildFromFile,
        }

        public event EventHandler OnSwich;

        public class SwitchEventArgs : EventArgs
        {

            public ISwitchElement Page { get; private set; }

            public SwitchEventArgs(ISwitchElement pPage)
            {
                Page = pPage;
            }
        }
    }

    public interface ISwitchElement
    {
        IViewModel ViewModel { get; }
    }



}
