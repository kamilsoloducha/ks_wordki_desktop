using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Wordki.Helpers {
  class CursorAtEndBehavior : Behavior<UIElement> {
    private TextBox _textBox;

    protected override void OnAttached() {
      base.OnAttached();
      _textBox = AssociatedObject as TextBox;

      if (_textBox == null) {
        return;
      }
      _textBox.TextChanged += TextBoxOnTextChanged;
    }

    private void TextBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs) {
      _textBox.CaretIndex = _textBox.Text.Length;
    }

    protected override void OnDetaching() {
      if (_textBox == null) {
        return;
      }
      _textBox.TextChanged -= TextBoxOnTextChanged;
      base.OnDetaching();
    }
  }
}
