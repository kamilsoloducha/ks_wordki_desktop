using System.Windows.Controls;
using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views
{
    public class PageBase : UserControl, ISwitchElement
    {

        protected readonly IViewModel viewModel;
        public IViewModel ViewModel { get { return viewModel; } }

        public PageBase(IViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = ViewModel;
            Loaded += ((s, e) => viewModel.Loaded());
            Unloaded += ((s, e) => viewModel.Unloaded());
        }

    }
}
