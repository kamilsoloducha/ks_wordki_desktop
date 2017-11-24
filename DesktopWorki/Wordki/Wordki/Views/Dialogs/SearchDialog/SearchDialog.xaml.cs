using System.Windows;

namespace Wordki.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SearchDialog.xaml
    /// </summary>
    public partial class SearchDialog : DialogBase
    {
        public SearchDialog() : base()
        {
            InitializeComponent();
            SearchingWord_TextBox.Focus();
        }
    }
}
