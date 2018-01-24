using Wordki.ViewModels;

namespace Wordki.Views
{
    public partial class BuildFromFilePage : PageBase
    {

        public BuildFromFilePage() : base(new BuildFromFileViewModel())
        {
            InitializeComponent();
        }
    }
}
