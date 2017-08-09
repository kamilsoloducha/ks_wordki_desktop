using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wordki.Controls {
  /// <summary>
  /// Interaction logic for NumericPicker.xaml
  /// </summary>
  public partial class NumericPicker : UserControl {

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
      "Value", typeof(int), typeof(NumericPicker), new FrameworkPropertyMetadata(0, ((d, e) => {
        NumericPicker lControl = d as NumericPicker;
        if (lControl == null) return;
        lControl.TextBox.Text = (string)e.NewValue;
      })));

    public int Value {
      get { return (int)GetValue(ValueProperty); }
      set { SetValue(ValueProperty, value); }
    }

    public NumericPicker() {
      InitializeComponent();
    }

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
      e.Handled = !IsTextAllowed(e.Text);
    }
    private static bool IsTextAllowed(string text) {
      Regex regex = new Regex("0-9");
      return !regex.IsMatch(text);
    }

    private void ButtonMoreClick(object sender, RoutedEventArgs e) {
      Value++;
    }

    private void ButtonLessClick(object sender, RoutedEventArgs e) {
      Value--;
    }
  }
}
