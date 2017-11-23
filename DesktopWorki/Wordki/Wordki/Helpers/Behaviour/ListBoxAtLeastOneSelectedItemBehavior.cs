using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Wordki.Helpers
{
    public class ListBoxAtLeastOneSelectedItemBehavior : Behavior<ListBox>
    {

        private ListBox _listBox;

        protected override void OnAttached()
        {
            base.OnAttached();
            _listBox = AssociatedObject as ListBox;

            if (_listBox == null)
            {
                return;
            }
            _listBox.SelectionChanged += SelectionChanged;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_listBox.SelectedItems.Count == 0 && e.RemovedItems.Count > 0)
            {
                _listBox.SelectedItems.Add(e.RemovedItems[0]);
            }
        }

        protected override void OnDetaching()
        {
            if (_listBox == null)
            {
                return;
            }
            _listBox.SelectionChanged += SelectionChanged;
            base.OnDetaching();
        }
    }
}
