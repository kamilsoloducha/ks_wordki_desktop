using System;
using Wordki.Helpers;
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

        private void GroupsDataGrid_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            Console.WriteLine($"ExtentHeight - {e.ExtentHeight}");
            Console.WriteLine($"VerticalOffset - {e.VerticalOffset}");
            Console.WriteLine($"VerticalChange - {e.VerticalChange}");
            Console.WriteLine();
            if (e.ExtentHeight <= 0)
            {
                return;
            }



        }
    }
}
