using Wordki.ViewModels;

namespace Wordki.Views
{
    public partial class PlotPage : PageBase
    {
        public PlotPage() : base(new ChartViewModel())
        {
            InitializeComponent();
        }
    }
}
