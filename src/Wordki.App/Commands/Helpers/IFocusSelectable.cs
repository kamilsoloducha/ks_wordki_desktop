using System.Collections.ObjectModel;

namespace Wordki.Commands
{
    public interface IFocusSelectable
    {

        ObservableCollection<bool> Focusable { get; }

    }
}
