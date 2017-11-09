using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Wordki.Helpers
{
    public class CursorAtEndBehavior : Behavior<UIElement>
    {
        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.Register("Enabled", typeof(bool), typeof(CursorAtEndBehavior), new PropertyMetadata(false));


        private TextBox _textBox;

        protected override void OnAttached()
        {
            base.OnAttached();
            _textBox = AssociatedObject as TextBox;

            if (_textBox == null)
            {
                return;
            }
            _textBox.TextChanged += TextBoxOnTextChanged;
        }

        private void TextBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (Enabled)
            {
                _textBox.CaretIndex = _textBox.Text.Length;
            }
        }

        protected override void OnDetaching()
        {
            if (_textBox == null)
            {
                return;
            }
            _textBox.TextChanged -= TextBoxOnTextChanged;
            base.OnDetaching();
        }
    }
}
