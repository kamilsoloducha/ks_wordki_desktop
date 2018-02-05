using Wordki.ViewModels;

namespace Wordki.Views
{
    public partial class GroupsPage : PageBase
    {

        private static GroupManagerViewModel localViewModel;

        public GroupsPage() : base(new GroupManagerViewModel())
        {
            InitializeComponent();
            localViewModel = viewModel as GroupManagerViewModel;
        }

    }
}
