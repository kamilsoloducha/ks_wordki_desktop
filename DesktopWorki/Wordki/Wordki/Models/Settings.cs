using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Wordki.Helpers;

namespace Wordki.Models {
  [Serializable]
  public class Settings : INotifyPropertyChanged {

    private const string FileName = "Settings.dat";
    private const string DirectoryPath = "Settings";

    /// <summary>
    /// Przechowuje obiekt ustawień - singleton
    /// </summary>
    private static Settings _settigns;

    #region Properties
    private int _fontSizeWord;
    public int FontSize {
      get { return _fontSizeWord; }
      set {
        if (value != _fontSizeWord) {
          if (value < 1) {
            value = 1;
          }
          _fontSizeWord = value;
          OnPropertyChanged();
        }
      }
    }

    private long _lastUserId;
    public long LastUserId {
      get { return _lastUserId; }
      set {
        if (_lastUserId == value) return;
        _lastUserId = value;
        OnPropertyChanged();
      }
    }

    private bool _fontSizeSensitive;
    public bool FontSizeSensitive {
      get { return _fontSizeSensitive; }
      set {
        if (_fontSizeSensitive == value) return;
        _fontSizeSensitive = value;
        OnPropertyChanged();
      }
    }

    private bool _showCommentsBefore;
    public bool ShowCommentsBefore {
      get { return _showCommentsBefore; }
      set {
        if (_showCommentsBefore == value) return;
        _showCommentsBefore = value;
        OnPropertyChanged();
      }
    }

    private ObservableCollection<ForeignLetter> _shortCuts;
    public ObservableCollection<ForeignLetter> ShortCuts {
      get { return _shortCuts; }
      set {
        if (_shortCuts == value) return;
        _shortCuts = value;
        OnPropertyChanged();
      }
    }

    private ForeignLetter _selectedShortCut;
    public ForeignLetter SelectedShortCut {
      get { return _selectedShortCut; }
      set {
        if (_selectedShortCut == value) return;
        _selectedShortCut = value;
        OnPropertyChanged();
      }
    }

    public ApplicationStyleEnum ApplicationStyle { get; set; }
    #endregion

    private Settings() {
      FontSize = 30;
      ApplicationStyle = ApplicationStyleEnum.Light;
      FontSizeSensitive = false;
      ShortCuts = new ObservableCollection<ForeignLetter> {
        new ForeignLetter {
          ForeignKey = "ś",
          KeyboardKey = 's'
        },
        new ForeignLetter {
          ForeignKey = "ź",
          KeyboardKey = 'x'
        },
        new ForeignLetter {
          ForeignKey = "ż",
          KeyboardKey = 'z'
        },
        new ForeignLetter {
          ForeignKey = "ą",
          KeyboardKey = 'a'
        },
        new ForeignLetter {
          ForeignKey = "ę",
          KeyboardKey = 'e'
        },
        new ForeignLetter {
          ForeignKey = "ó",
          KeyboardKey = 'o'
        },
        new ForeignLetter {
          ForeignKey = "ł",
          KeyboardKey = 'l'
        },
        new ForeignLetter {
          ForeignKey = "ń",
          KeyboardKey = 'n'
        }
      };
      //foreach (ForeignLetter letter in ShortCuts) {

      //  Console.Write("{0} - {1} - {2}", letter.KeyboardKey, letter.ForeignKey, (int)Convert.ToChar(letter.ForeignKey));
      //  //Console.Write("{0} - {1}", letter.KeyboardKey, letter.ForeignKey);
      //  //foreach (byte value in Encoding.UTF8.GetBytes()) {
      //  //  Console.Write("[ {0} ]", value);
      //  //}
      //  Console.WriteLine();
      //}

      //for (int i = 255; i < 500; i++) {
      //  Console.WriteLine("{0} - {1}", i, char.ConvertFromUtf32(i));
      //}
    }

    /// <summary>
    /// Zwraca obiekt ustawien
    /// </summary>
    /// <returns>Obiekt ustawien</returns>
    public static Settings GetSettings() {
      if (_settigns != null)
        return _settigns;
      try {
        if (LoadSettings()) {//proba odczytu ustawien
          if (_settigns == null)
            return new Settings();
        }
        ChangeStyle(_settigns.ApplicationStyle);
      } catch (Exception lException) {
        Logger.LogError("{0} - {1}", "Settings.GetSettings", lException.Message);
      }
      if (_settigns != null)
        return _settigns;
      return new Settings();
    }

    private void CheckDirectoryPath() {
      string lSettingsDirectory = Path.Combine(Directory.GetCurrentDirectory(), DirectoryPath);
      if (!Directory.Exists(lSettingsDirectory)) {
        Directory.CreateDirectory(lSettingsDirectory);
      }
    }

    /// <summary>
    /// Ladowanie ustawien z pliku
    /// </summary>
    /// <returns>True jezeli sie udalo zaladowac ustawienia</returns>
    private static bool LoadSettings() {
      try {
        string lPath = Path.Combine(Directory.GetCurrentDirectory(), DirectoryPath, FileName);
        Serializer lSerializer = new Serializer();
        object lSerializeResult = lSerializer.DeserializeFromXml(lPath, typeof(Settings));
        _settigns = lSerializeResult != null ? (Settings)lSerializeResult : null;
      } catch (Exception lException) {
        Logger.LogError("Blad w {0} - {1}", "LoadSettings", lException.Message);
      }
      if (_settigns != null)
        return true;
      _settigns = new Settings();
      return false;
    }

    public void SaveSettings() {
      CheckDirectoryPath();
      string lPath = Path.Combine(Directory.GetCurrentDirectory(), DirectoryPath, FileName);
      Serializer lSerializer = new Serializer();
      lSerializer.SerializeToXml(_settigns, lPath);
    }

    public void ResetSettings() {
      CheckDirectoryPath();
      _settigns = null;
      File.Delete(Path.Combine(Directory.GetCurrentDirectory(), DirectoryPath, FileName));
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string pPropertyName = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
      }
    }

    public static void ChangeStyle(int index) {
      switch (index) {
        case 0:
          ChangeStyle(ApplicationStyleEnum.Light);
          break;
        case 1:
          ChangeStyle(ApplicationStyleEnum.Dark);
          break;
      }
    }

    public static void ChangeStyle(ApplicationStyleEnum pStyleEnum) {
      _settigns.ApplicationStyle = pStyleEnum;
      switch (pStyleEnum) {
        case ApplicationStyleEnum.Light:
          App.Current.Resources["UsedNormalBrush"] = App.Current.Resources["LightNormalBrush"];
          App.Current.Resources["UsedHoverBrush"] = App.Current.Resources["LightHoverBrush"];
          App.Current.Resources["UsedPressedBrush"] = App.Current.Resources["LightPressedBrush"];
          App.Current.Resources["UsedNormalFrontBrush"] = App.Current.Resources["LightNormalFrontBrush"];
          App.Current.Resources["UsedUnableFrontBrush"] = App.Current.Resources["LightUnableFrontBrush"];

          App.Current.Resources["UsedNormalColor"] = App.Current.Resources["LightNormalColor"];
          App.Current.Resources["UsedHoverColor"] = App.Current.Resources["LightHoverColor"];
          App.Current.Resources["UsedPressedColor"] = App.Current.Resources["LightPressedColor"];
          App.Current.Resources["UsedNormalFrontColor"] = App.Current.Resources["LightNormalFrontColor"];
          App.Current.Resources["UsedUnableFrontColor"] = App.Current.Resources["LightUnableFrontColor"];

          break;
        case ApplicationStyleEnum.Dark:
          App.Current.Resources["UsedNormalBrush"] = App.Current.Resources["DarkNormalBrush"];
          App.Current.Resources["UsedHoverBrush"] = App.Current.Resources["DarkHoverBrush"];
          App.Current.Resources["UsedPressedBrush"] = App.Current.Resources["DarkPressedBrush"];
          App.Current.Resources["UsedNormalFrontBrush"] = App.Current.Resources["DarkNormalFrontBrush"];
          App.Current.Resources["UsedUnableFrontBrush"] = App.Current.Resources["DarkUnableFrontBrush"];

          App.Current.Resources["UsedNormalColor"] = App.Current.Resources["DarkNormalColor"];
          App.Current.Resources["UsedHoverColor"] = App.Current.Resources["DarkHoverColor"];
          App.Current.Resources["UsedPressedColor"] = App.Current.Resources["DarkPressedColor"];
          App.Current.Resources["UsedNormalFrontColor"] = App.Current.Resources["DarkNormalFrontColor"];
          App.Current.Resources["UsedUnableFrontColor"] = App.Current.Resources["DarkUnableFrontColor"];
          break;
        default:
          throw new ArgumentOutOfRangeException("pStyleEnum");
      }
    }
  }

  public enum ApplicationStyleEnum {
    Dark,
    Light
  }
}
