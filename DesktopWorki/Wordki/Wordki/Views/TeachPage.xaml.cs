using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views {
  /// <summary>
  /// Interaction logic for TeachPage.xaml
  /// </summary>
  public partial class TeachPage : ISwitchElement {
    
    private readonly IViewModel viewModel;

    public TeachPage() {
      InitializeComponent();
      viewModel = new TeachViewModel();
      DataContext = viewModel;
    }

    public IViewModel ViewModel { get { return viewModel; } }

  }
}
