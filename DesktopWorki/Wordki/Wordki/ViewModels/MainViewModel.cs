using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Util;
using Wordki.Database;
using Wordki.InteractionProvider;

namespace Wordki.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public ICommand SearchWordCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

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
            Switcher.LockStates.Add(Helpers.Switcher.State.Login);
            Switcher.LockStates.Add(Helpers.Switcher.State.Register);
            Switcher.LockStates.Add(Helpers.Switcher.State.Menu);
            Switcher.LockStates.Add(Helpers.Switcher.State.TeachTyping);
            Switcher.OnSwich += (sender, args) =>
            {
                Helpers.ISwitchElement page = ((Helpers.Switcher.SwitchEventArgs)args).Page;
                if (page != null)
                {
                    Page = page;
                    page.ViewModel.InitViewModel();
                }
            };
            SearchWordCommand = new BuilderCommand(SearchWord);
            BackCommand = new BuilderCommand(Back);
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
