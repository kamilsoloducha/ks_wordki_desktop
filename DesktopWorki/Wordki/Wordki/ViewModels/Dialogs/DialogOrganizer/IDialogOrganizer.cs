using System.Windows;

namespace Wordki.ViewModels.Dialogs
{
    public interface IDialogOrganizer
    {

        void HideDialog();
        void ShowDialog(Window window);
    }
}