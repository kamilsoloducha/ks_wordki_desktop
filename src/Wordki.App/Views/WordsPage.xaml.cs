using Wordki.ViewModels;

namespace Wordki.Views
{
    public partial class WordsPage : PageBase
    {

        public WordsPage() : base(new WordManageViewModel())
        {
            InitializeComponent();
        }

    }
}
