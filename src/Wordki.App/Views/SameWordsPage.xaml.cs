using Wordki.ViewModels;

namespace Wordki.Views
{
    /// <summary>
    /// Interaction logic for SameWordsPage.xaml
    /// </summary>
    public partial class SameWordsPage : PageBase { 
    public SameWordsPage() :base(new SameWordsViewModel()){
      InitializeComponent();
    }
  }
}
