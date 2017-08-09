using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views {
  /// <summary>
  /// Interaction logic for BuilderPage.xaml
  /// </summary>
  public partial class BuilderPage : ISwitchElement {

    private readonly IViewModel viewModel;

    public BuilderPage() {
      InitializeComponent();
      viewModel = new BuilderViewModel();
      DataContext = viewModel;
    }

    public IViewModel ViewModel {
      get { return viewModel; }
    }
  }
}
