using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : PageBase
    {

        public MenuPage() : base(new MainMenuViewModel())
        {
            InitializeComponent();
        }
    }
}
