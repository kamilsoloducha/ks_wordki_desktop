using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Wordki.Controls {
  class DataGridExtension : DataGrid {
    public DataGridExtension()
    {
      this.SelectionChanged += DataGridExtension_SelectionChanged;
    }

    void DataGridExtension_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        this.SelectedItemsList = this.SelectedItems;
    }
    #region SelectedItemsList

    public IList SelectedItemsList
    {
        get { return (IList)GetValue (SelectedItemsListProperty); }
        set { SetValue (SelectedItemsListProperty, value); }
    }

    public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(DataGridExtension), new PropertyMetadata(null));

    #endregion
  }
}
