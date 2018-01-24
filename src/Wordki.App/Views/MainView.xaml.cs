using System;
using System.Windows;
using System.Windows.Input;
using Controls.Notification;
using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {

        private Switcher Switcher { get; set; }
        private MainViewModel ViewModel { get; set; }



        public MainView()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;

            //Switcher = new Switcher();
            //Switcher.LockStates.Add(Switcher.State.Login);
            //Switcher.LockStates.Add(Switcher.State.Register);
            //Switcher.LockStates.Add(Switcher.State.Menu);
            //Switcher.LockStates.Add(Switcher.State.TeachTyping);
            //Switcher.OnSwich += (sender, args) =>
            //{
            //    ISwitchElement page = ((Switcher.SwitchEventArgs)args).Page;
            //    if (page != null)
            //    {
            //        Page.Content = page;
            //        page.ViewModel.InitViewModel();
            //        Page.Focus();
            //    }
            //};
            Toaster.Instance = ToastList;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            ViewModel.Switcher.Switch(Switcher.State.Login);
            //Switcher.Switch(Switcher.State.Login);
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Switcher.Back();
        }
    }
}
