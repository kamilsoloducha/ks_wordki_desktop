using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wordki.Controls {
  /// <summary>
  /// Interaction logic for DrawerSign.xaml
  /// </summary>
  public partial class DrawerSign : UserControl {
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
      "Text", typeof(string), typeof(DrawerSign), new FrameworkPropertyMetadata(string.Empty, ((d, e) => {
        DrawerSign lControl = d as DrawerSign;
        if (lControl == null) return;
        lControl.TextBlock.Text = (string)e.NewValue;
      })));

    public static readonly DependencyProperty IsLightProperty = DependencyProperty.Register(
      "IsLight", typeof(bool), typeof(DrawerSign), new FrameworkPropertyMetadata(false, ((d, e) => {
        DrawerSign lControl = d as DrawerSign;
        if (lControl == null) return;
        bool lIsLight = (bool)e.NewValue;
        if (lIsLight) {
          lControl.Border.Background = lControl.LightBackground;
        } else {
          lControl.Border.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }
      })));

    public static readonly DependencyProperty LightBackgroundProperty = DependencyProperty.Register(
      "LightProperty", typeof(SolidColorBrush), typeof(DrawerSign), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));

    protected override void OnRender(DrawingContext drawingContext) {
      base.OnRender(drawingContext);
      if (ActualHeight > ActualWidth) Height = ActualWidth;
      else Width = ActualHeight;
    }

    public string Text {
      get { return GetValue(TextProperty).ToString(); }
      set { SetValue(TextProperty, value); }
    }

    public bool? IsLight {
      get { return GetValue(IsLightProperty) as bool?; }
      set { SetValue(IsLightProperty, value); }
    }

    public SolidColorBrush LightBackground {
      get { return GetValue(LightBackgroundProperty) as SolidColorBrush; }
      set { SetValue(LightBackgroundProperty, value); }
    }

    public DrawerSign() {
      InitializeComponent();
    }
  }
}
