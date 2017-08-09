using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views {
  /// <summary>
  /// Interaction logic for LoginPage.xaml
  /// </summary>
  public partial class LoginPage : ISwitchElement {

    private readonly IViewModel viewModel;

    public LoginPage() {
      InitializeComponent();
      viewModel = new LoginViewModel();
      DataContext = viewModel;
    }

    public IViewModel ViewModel { get { return viewModel; } }

  }
}
