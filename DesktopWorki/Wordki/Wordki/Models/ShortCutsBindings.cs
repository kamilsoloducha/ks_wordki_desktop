using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Wordki.Models {
  public class ShortCutsBindings {

    public static ObservableCollection<KeyBinding> GetBindings(IList<ForeignLetter> shortCutList, ICommand command) {
      ObservableCollection<KeyBinding> bindings = new ObservableCollection<KeyBinding>();
      foreach (ForeignLetter letter in shortCutList) {
        Key key = (Key)Enum.Parse(typeof(Key), letter.KeyboardKey.ToString().ToUpper());
        bindings.Add(new KeyBinding {
          Key = key, 
          Command = command,
          CommandParameter = letter.ForeignKey,
          Modifiers = ModifierKeys.Alt
        });
      }
      return bindings;
    }

  }
}
