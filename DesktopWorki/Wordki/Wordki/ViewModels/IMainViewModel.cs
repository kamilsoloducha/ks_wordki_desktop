using System;

namespace Wordki.ViewModels {
  public interface IMainViewModel {
    Action ActionHide { get; set; }
    Action ActionShow { get; set; }
    Action ActionClose { get; set; }
  }
}
