using System.Collections.Generic;

namespace Wordki.Helpers {
  public class Phrase {
    public string text { get; set; }
    public string language { get; set; }
  }

  public class Meaning {
    public string language { get; set; }
    public string text { get; set; }
  }

  public class Tuc {
    public Phrase phrase { get; set; }
    public List<Meaning> meanings { get; set; }
    public object meaningId { get; set; }
    public List<int> authors { get; set; }
  }

  public class Author {
    public string U { get; set; }
    public int id { get; set; }
    public string N { get; set; }
    public string url { get; set; }
  }

  public class RootObject {
    public string result { get; set; }
    public List<Tuc> tuc { get; set; }
  }
}