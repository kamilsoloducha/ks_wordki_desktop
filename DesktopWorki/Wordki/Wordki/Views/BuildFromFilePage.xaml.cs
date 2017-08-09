using System.Windows.Controls;
using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views {
  /// <summary>
  /// Interaction logic for BuildFromFilePage.xaml
  /// </summary>
  public partial class BuildFromFilePage : UserControl, ISwitchElement {

    private readonly IViewModel viewModel;

    public BuildFromFilePage() {
      InitializeComponent();
      viewModel = new BuildFromFileViewModel();
      DataContext = viewModel;
    }

    public IViewModel ViewModel {
      get { return viewModel; }
    }
  }
}
