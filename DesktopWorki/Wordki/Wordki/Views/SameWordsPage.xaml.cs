using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views {
  /// <summary>
  /// Interaction logic for SameWordsPage.xaml
  /// </summary>
  public partial class SameWordsPage : ISwitchElement {
    private readonly IViewModel viewModel;
    public SameWordsPage() {
      InitializeComponent();
      viewModel = new SameWordsViewModel();
      DataContext = viewModel;
    }
    public IViewModel ViewModel { get { return viewModel; } }
  }
}
