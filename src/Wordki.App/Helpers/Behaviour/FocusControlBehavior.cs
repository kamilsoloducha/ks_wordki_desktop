using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Wordki.Helpers
{
    public class FocusControlBehavior : Behavior<UIElement>
    {

        public bool Focused
        {
            get { return (bool)GetValue(FocusedProperty); }
            set { SetValue(FocusedProperty, value); }
        }

        public static readonly DependencyProperty FocusedProperty =
            DependencyProperty.Register("Focused", typeof(bool), typeof(FocusControlBehavior), new PropertyMetadata(false, OnPropertyChangedCallback));

        private static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;
            if (newValue && (d as FocusControlBehavior).Focused != newValue)
            {
                (d as FocusControlBehavior).uiElement.Focus();
            }
        }

        private UIElement uiElement;

        protected override void OnAttached()
        {
            base.OnAttached();
            uiElement = AssociatedObject;

            if (uiElement == null)
            {
                return;
            }
            uiElement.GotFocus += UiElement_GotFocus;
            uiElement.LostFocus += UiElement_LostFocus;
        }

        private void UiElement_LostFocus(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("lost focus");
            Focused = false;
        }

        private void UiElement_GotFocus(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("got focus");
            Focused = true;
        }

        protected override void OnDetaching()
        {
            if (uiElement == null)
            {
                return;
            }
            uiElement.GotFocus -= UiElement_GotFocus;
            uiElement.LostFocus -= UiElement_LostFocus;
            base.OnDetaching();
        }

    }
}
