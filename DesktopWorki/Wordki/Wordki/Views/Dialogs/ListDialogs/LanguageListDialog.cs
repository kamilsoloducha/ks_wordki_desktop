using System.Windows;

namespace Wordki.Views.Dialogs
{
    public class LanguageListDialog : ListDialog
    {
        public LanguageListDialog() : base()
        {
            ListBox.ItemContainerStyle = (Style)Application.Current.Resources["LanguageChooseListBoxStyle"];
        }

    }
}
