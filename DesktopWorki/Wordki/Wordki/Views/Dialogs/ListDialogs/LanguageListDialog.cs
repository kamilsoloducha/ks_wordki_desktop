using System.Windows;

namespace Wordki.Views.Dialogs.ListDialogs {
  public class LanguageListDialog : ListDialog{

    public LanguageListDialog() : base(){
      ItemStyle = (Style) Application.Current.Resources["LanguageChooseListBoxStyle"];
    }

  }
}
