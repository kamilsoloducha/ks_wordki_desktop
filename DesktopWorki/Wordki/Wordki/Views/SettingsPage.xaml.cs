using Wordki.ViewModels;

namespace Wordki.Views {
  public partial class SettingsPage : PageBase {
    public SettingsPage() :base(new SettingsViewModel()){
      InitializeComponent();
    }
  }
}
