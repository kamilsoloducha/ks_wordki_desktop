using Wordki.ViewModels;

namespace Wordki.Views
{
    public partial class LoginPage : PageBase
    {
        public LoginPage() : base(new LoginViewModel())
        {
            InitializeComponent();
        }
    }
}
