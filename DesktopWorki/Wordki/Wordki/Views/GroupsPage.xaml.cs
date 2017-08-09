using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views {
  /// <summary>
  /// Interaction logic for GroupsPage.xaml
  /// </summary>
  public partial class GroupsPage : ISwitchElement {
    private readonly IViewModel viewModel;
    public GroupsPage() {
      InitializeComponent();
      viewModel = new GroupManagerViewModel();
      DataContext = viewModel;
    }

    public IViewModel ViewModel {
      get { return viewModel; }
    }
  }
}
