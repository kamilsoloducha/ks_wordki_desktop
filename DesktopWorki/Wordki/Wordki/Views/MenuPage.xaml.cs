using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views {
  /// <summary>
  /// Interaction logic for MenuPage.xaml
  /// </summary>
  public partial class MenuPage : ISwitchElement {

    private readonly IViewModel viewModel;

    public MenuPage() {
      InitializeComponent();
      viewModel = new MainMenuViewModel();
      DataContext = viewModel;

    }

    public IViewModel ViewModel { get { return viewModel; } }

  }
}
