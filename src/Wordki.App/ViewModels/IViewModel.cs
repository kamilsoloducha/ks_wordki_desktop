using Wordki.Helpers;

namespace Wordki.ViewModels
{
    public interface IViewModel
    {
        Switcher Switcher { get; set; }
        void InitViewModel(object parameter = null);
        void Back();
        void Loaded();
        void Unloaded();
    }
}
