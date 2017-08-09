using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wordki.Helpers {
  public static class FocusExtension {
    public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached(
      "IsFocused", typeof(bool), typeof(FocusExtension), new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

    private static void OnIsFocusedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
      var lUiElement = (UIElement)obj;
      if ((bool)e.NewValue) {
        lUiElement.Focus();
      }
    }

    public static bool GetIsFocused(DependencyObject obj) {
      return (bool)obj.GetValue(IsFocusedProperty);
    }

    public static void SetIsFocused(DependencyObject obj, bool value) {
      obj.SetValue(IsFocusedProperty, value);
    }
  }

  public static class ImageExtension {
    public static readonly DependencyProperty ImageExtensionProperty =
      DependencyProperty.RegisterAttached("ImageExtension", typeof(string), typeof(ImageExtension),
        new PropertyMetadata(""));

    public static string GetImageExtension(DependencyObject obj) {
      return obj.GetValue(ImageExtensionProperty).ToString();
    }

    public static void SetImageExtension(DependencyObject obj, string value) {
      obj.SetValue(ImageExtensionProperty, value);
    }
  }

  public class ImageButton : Button {
    static ImageButton() {
    }

    public ImageSource ImageSource {
      get { return (ImageSource) GetValue(ImageSourceProperty); }
      set { SetValue(ImageSourceProperty, value);}
    }

    public static readonly DependencyProperty ImageSourceProperty =
      DependencyProperty.RegisterAttached("ImageSource", typeof(ImageSource), typeof(ImageButton),
        new UIPropertyMetadata(null));
  }
}
