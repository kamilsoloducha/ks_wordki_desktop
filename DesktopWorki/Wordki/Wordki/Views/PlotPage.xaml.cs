using Wordki.Helpers;
using Wordki.ViewModels;

namespace Wordki.Views {
  /// <summary>
  /// Interaction logic for PlotPage.xaml
  /// </summary>
  public partial class PlotPage : ISwitchElement {
    private readonly IViewModel viewModel;
    public PlotPage() {
      InitializeComponent();
      viewModel = new ChartViewModel();
      DataContext = viewModel;
    }

    public IViewModel ViewModel { get { return viewModel; } }
  }
}
