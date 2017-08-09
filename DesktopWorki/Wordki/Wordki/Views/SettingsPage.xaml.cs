using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views {
  /// <summary>
  /// Interaction logic for SettingsPage.xaml
  /// </summary>
  public partial class SettingsPage : ISwitchElement {
    private readonly IViewModel viewModel;
    public SettingsPage() {
      InitializeComponent();
      viewModel = new SettingsViewModel();
      DataContext = viewModel;
    }
  
    public IViewModel ViewModel { get { return viewModel; } }
  }
}
