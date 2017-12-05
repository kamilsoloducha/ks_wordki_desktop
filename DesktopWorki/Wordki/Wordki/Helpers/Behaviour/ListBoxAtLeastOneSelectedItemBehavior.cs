using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Wordki.Helpers
{
    public class ListBoxAtLeastOneSelectedItemBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject == null)
            {
                return;
            }
            AssociatedObject.SelectionChanged += SelectionChanged;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(AssociatedObject.SelectedItems.Count == 0 && e.RemovedItems.Count > 0)
            {
                AssociatedObject.SelectedItems.Add(e.RemovedItems[0]);
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject == null)
            {
                return;
            }
            AssociatedObject.SelectionChanged += SelectionChanged;
            base.OnDetaching();
        }
    }
}
