using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views
{
    /// <summary>
    /// Interaction logic for BuilderPage.xaml
    /// </summary>
    public partial class BuilderPage : PageBase
    {
        public BuilderPage() : base(new BuilderViewModel())
        {
            InitializeComponent();
        }
    }
}
