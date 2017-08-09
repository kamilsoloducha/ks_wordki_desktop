namespace Wordki.Models {
  public enum TranslationDirection {
    Unknown = 0,
    FromFirst = 1,
    FromSecond = 2,
  }
  public enum ConnectMode {
    Manual,
    Remind,
    Automatic,
  }
  public enum LanguageType {
    Unknown = 0,
    English = 1,//eng
    Polish = 2,//pol
    Germany = 3,//deu
    French = 4,//fra
    Spanish = 5,//esp
    Portuaglese = 6,//por
    Russian = 7,//rus
    Italian = 8,//ita
  }
}
