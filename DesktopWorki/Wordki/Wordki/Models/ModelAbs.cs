using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Wordki.Models.StateManager;

namespace Wordki.Models {
  public abstract class ModelAbs<T> : INotifyPropertyChanged, ICloneable{

    [JsonIgnore]
    protected static readonly IStateManager<T> StateManager = new StateManager<T>();

    public object Clone() {
      return MemberwiseClone();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(name));
      }
    }
  }
}
