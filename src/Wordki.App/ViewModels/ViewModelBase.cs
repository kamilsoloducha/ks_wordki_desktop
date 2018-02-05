using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wordki.Helpers;

namespace Wordki.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IViewModel
    {
        public Switcher Switcher { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public abstract void Back();

        public abstract void InitViewModel(object parameter = null);

        public abstract void Loaded();

        public abstract void Unloaded();
    }
}
