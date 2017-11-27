namespace Wordki.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for TranslationListDialog.xaml
    /// </summary>
    public partial class TranslationListDialog : DialogBase
    {
        public TranslationListDialog()
        {
            InitializeComponent();
            Height = Owner.ActualWidth / 3;
        }
    }
}
