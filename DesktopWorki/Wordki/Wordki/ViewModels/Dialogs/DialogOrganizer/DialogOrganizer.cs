using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wordki.Views.Dialogs;

namespace Wordki.ViewModels.Dialogs
{
    public class DialogOrganizer : IDialogOrganizer
    {
        public Window Dialog { get; set; }

        public void HideDialog()
        {
            if (Dialog != null)
            {
                Dialog.Close();
            }
        }

        public void ShowDialog(Window window)
        {
            if (Dialog != null && Dialog.IsVisible)
            {
                Dialog.Close();
            }
            Dialog = window;
            window.ShowDialog();
        }
    }
}
