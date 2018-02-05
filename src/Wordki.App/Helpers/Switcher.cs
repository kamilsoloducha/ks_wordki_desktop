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
        private readonly Stack<State> pageStack = new Stack<State>();
        private readonly Dictionary<State, ISwitchElement> pageDictionary = new Dictionary<State, ISwitchElement>();

        public void Switch(State newState, object parameter = null)
        {
            pageStack.Push(newState);
            ISwitchElement newPage;
            if (pageDictionary.ContainsKey(newState))
            {
                newPage = pageDictionary[newState];
            }
            else
            {
                newPage = PageFactory(newState);
                pageDictionary.Add(newState, newPage);
            }
            if (OnSwich != null)
            {
                OnSwich.Invoke(this, new SwitchEventArgs(newPage, parameter));
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
                case State.Same:
                    result = new SameWordsPage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("pState");
            }
            result.ViewModel.Switcher = this;
            return result;
        }

        public void Back(bool pForce = false)
        {
            if (!pForce && LockStates.Contains(pageStack.Peek()))
            {
                return;
            }
            if (pageStack.Count == 1)
            {
                Reset();
                return;
            }
            pageDictionary[pageStack.Pop()].ViewModel.Back();
            Switch(pageStack.Pop());
        }

        public void Reset()
        {
            pageStack.Clear();
            pageDictionary.Clear();
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

        public event EventHandler<SwitchEventArgs> OnSwich;

        public class SwitchEventArgs : EventArgs
        {

            public ISwitchElement Page { get; private set; }
            public object Parameter { get; private set; }

            public SwitchEventArgs(ISwitchElement pPage, object parameter = null)
            {
                Page = pPage;
                Parameter = parameter;
            }
        }
    }

    public interface ISwitchElement
    {
        IViewModel ViewModel { get; }
    }



}
