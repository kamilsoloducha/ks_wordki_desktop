using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views {
  /// <summary>
  /// Interaction logic for RegisterPage.xaml
  /// </summary>
  public partial class RegisterPage : ISwitchElement {
    private readonly IViewModel viewModel;

    public RegisterPage() {
      InitializeComponent();
      viewModel = new RegisterViewModel();
      DataContext = viewModel;
    }

    public IViewModel ViewModel { get { return viewModel; } }
  }
}
