
using System.ComponentModel;

namespace Wordki.Helpers {
  public class CancelableWorker {

    public delegate void DoWorkDelegate();

    public DoWorkDelegate DoWork { get; set; }
    public volatile bool ShouldStop;

    public void Start() {
      if (DoWork != null)
      {
        
      }
    }




  }
}
