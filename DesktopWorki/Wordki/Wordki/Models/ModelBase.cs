using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Wordki.Models.StateManager;

namespace Wordki.Models
{
    [Serializable]
    public abstract class ModelBase<T> : INotifyPropertyChanged, ICloneable
    {

        [JsonIgnore]
        protected static readonly IStateManager<T> StateManager = new StateManager<T>();

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
