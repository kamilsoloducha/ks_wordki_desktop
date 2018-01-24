using Wordki.ViewModels;

namespace Wordki.Views
{
    public partial class RegisterPage : PageBase
    {

        public RegisterPage() : base(new RegisterViewModel())
        {
            InitializeComponent();
        }
    }
}
