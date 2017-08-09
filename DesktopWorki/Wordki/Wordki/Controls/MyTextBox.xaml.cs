using System.Windows.Controls;

namespace Wordki.Controls
{
  /// <summary>
  /// Interaction logic for MyTextBox.xaml
  /// </summary>
  public partial class MyTextBox : UserControl
  {
    public string Hint {get; set;}
    public MyTextBox()
    {
      InitializeComponent();
    }
  }
}
