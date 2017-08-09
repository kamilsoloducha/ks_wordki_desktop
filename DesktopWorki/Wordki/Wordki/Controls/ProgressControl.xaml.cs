using System.Windows;
using System.Windows.Controls;

namespace Wordki.Controls {
  /// <summary>
  /// Interaction logic for ProgressControl.xaml
  /// </summary>
  public partial class ProgressControl : UserControl {
    public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(
      "Progress", typeof(double), typeof(ProgressControl), new FrameworkPropertyMetadata(0.0, ((d, e) => {
        ProgressControl lSource = d as ProgressControl;
        if (lSource == null) return;
        double lProgress = (double)e.NewValue;
        if (lProgress > 100) lProgress = 100;
        else if (lProgress < 0) lProgress = 0;
        lProgress /= 100;
        if (lSource.ActualWidth - 10 < 0) return;
        lSource.Rectangle.Width = (lSource.ActualWidth - 10) * lProgress;
      })));

    public double Progress {
      get { return (double)GetValue(ProgressProperty); }
      set { SetValue(ProgressProperty, value); }
    }

    public ProgressControl() {
      InitializeComponent();
    }
  }
}
