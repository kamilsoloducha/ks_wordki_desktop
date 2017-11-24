using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Wordki.Helpers;

namespace Wordki.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IViewModel
    {
        public Switcher Switcher { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name="")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public abstract void Back();

        public abstract void InitViewModel();

        public virtual void Loaded()
        {
            Console.WriteLine("Loaded");
        }

        public virtual void Unloaded()
        {
            Console.WriteLine("Unloaded");
        }
    }
}
