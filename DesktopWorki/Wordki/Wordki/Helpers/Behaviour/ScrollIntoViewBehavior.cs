
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Wordki.Helpers {
  public class ScrollIntoViewBehavior : Behavior<DataGrid> {
    protected override void OnAttached() {
      base.OnAttached();
      AssociatedObject.SelectionChanged += new SelectionChangedEventHandler(AssociatedObject_SelectionChanged);
    }
    void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (sender is DataGrid) {
        DataGrid grid = (sender as DataGrid);
        if (grid.SelectedItem != null) {
          grid.Dispatcher.Invoke(delegate {
            grid.UpdateLayout();
            grid.ScrollIntoView(grid.SelectedItem, null);
          });
        }
      }
    }
    protected override void OnDetaching() {
      base.OnDetaching();
      AssociatedObject.SelectionChanged -=
          new SelectionChangedEventHandler(AssociatedObject_SelectionChanged);
    }
  }
}
