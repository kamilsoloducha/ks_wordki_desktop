using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Util;
using Wordki.Database;
using Wordki.InteractionProvider;
using Wordki.Models;

namespace Wordki.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public ICommand SearchWordCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand StyleCommand { get; private set; }

        public Helpers.Switcher Switcher { get; private set; }

        private object page;
        public object Page
        {
            get { return page; }
            set
            {
                if(page == value)
                {
                    return;
                }
                page = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            Switcher = new Helpers.Switcher();
            Switcher.LoadStates();
            Switcher.LockStates.Add(Helpers.Switcher.State.Login);
            Switcher.LockStates.Add(Helpers.Switcher.State.Register);
            Switcher.LockStates.Add(Helpers.Switcher.State.Menu);
            Switcher.LockStates.Add(Helpers.Switcher.State.TeachTyping);
            Switcher.OnSwich += (sender, args) =>
            {
                Helpers.ISwitchElement page = args.Page;
                if (page != null)
                {
                    Page = page;
                    page.ViewModel.InitViewModel(args.Parameter);
                }
            };
            SearchWordCommand = new BuilderCommand(SearchWord);
            BackCommand = new BuilderCommand(Back);
            StyleCommand = new BuilderCommand(ChangeStyle);
        }

        private void ChangeStyle(object obj)
        {
            ResourceDictionary dict = App.Current.Resources.MergedDictionaries[0];
            if (dict.Source.ToString().Contains("Dark"))
            {
                dict.Source = new Uri("..\\Styles\\LightStyle.xaml", UriKind.Relative);
            }
            else
            {
                dict.Source = new Uri("..\\Styles\\DarkStyle.xaml", UriKind.Relative);
            }
        }

        private void Back(object obj)
        {
            Switcher.Back();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        private void SearchWord(object obj)
        {
            if (UserManagerSingleton.Instence.User == null)
            {
                //user not loged
                return;
            }
            ISearchProvider provider = new SearchProvider();
            provider.Interact();
        }
    }
}
