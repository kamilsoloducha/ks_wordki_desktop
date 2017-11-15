using System;
using System.ComponentModel;

namespace Wordki.Models.Connector {
  [Serializable]
  public class ApiResponse {
    
    [DefaultValue(true)]
    public bool IsError { get; set; }
    [DefaultValue("")]
    public string Message { get; set; }

    public ApiResponse() {
      IsError = true;
      Message = string.Empty;
    }
  }
}
