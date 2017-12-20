using Wordki.ViewModels.Dialogs;

namespace Wordki.InteractionProvider
{
    public interface IYesNoProvider : IInteractionProvider
    {
        DialogViewModelBase ViewModel { get; set; }
    }
}
