using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views {
  /// <summary>
  /// Interaction logic for WordsPage.xaml
  /// </summary>
  public partial class WordsPage : ISwitchElement {

    private readonly IViewModel viewModel;

    public WordsPage() {
      InitializeComponent();
      viewModel = new WordManageViewModel();
      DataContext = viewModel;
    }

    public IViewModel ViewModel {
      get { return viewModel; }
    }
  }
}
