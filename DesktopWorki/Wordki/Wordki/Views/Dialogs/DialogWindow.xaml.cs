using System;
using System.Windows;
using Wordki.ViewModels.Dialogs;

namespace Wordki.Views.Dialogs
{
    public partial class DialogWindow : Window
    {
        public DialogWindowViewModel ViewModel { get; set; }

        public event EventHandler OnHide;

        public DialogWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            Width = Owner.Width;
            ViewModel = new DialogWindowViewModel();
            ViewModel.ClosingRequest += Close;
            Closed += Close;
            DataContext = ViewModel;
            
        }

        private void Close(object sender, EventArgs args)
        {
            if(OnHide!= null)
            {
                OnHide.Invoke(this, new EventArgs());
            }
        }
    }
}
