using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views
{
    public partial class GroupsPage : PageBase
    {
        public GroupsPage() : base(new GroupManagerViewModel())
        {
            InitializeComponent();
        }
    }
}
