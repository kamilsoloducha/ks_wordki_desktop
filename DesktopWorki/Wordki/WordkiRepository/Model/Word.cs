using System;
using Newtonsoft.Json;
using Repository.Models;

namespace WordkiRepository.Model {
  [Serializable]
  public class Word : ModelAbs<IWord>, IWord {

    public long Id { get; set; }

    [JsonIgnore]
    public long UserId { get; set; }

    private long _groupId;
    public long GroupId {
      get { return _groupId; }
      set {
        if (_groupId == value)
          return;
        _groupId = value;
        State = StateManager.NewState(State);
      }
    }

    private string _language1;
    public string Language1 {
      get { return _language1; }
      set {
        if (_language1 == value)
          return;
        _language1 = value;
        OnPropertyChanged();
        State = StateManager.NewState(State);
      }
    }

    public bool ShouldSerializeLanguage1() {
      return StateManager.GetState(State, "Language1") > 0;
    }

    private string _language2;
    public string Language2 {
      get { return _language2; }
      set {
        if (_language2 == value)
          return;
        _language2 = value;
        OnPropertyChanged();
        State = StateManager.NewState(State);
      }
    }

    public bool ShouldSerializeLanguage2() {
      return StateManager.GetState(State, "Language2") > 0;
    }


    private byte _drawer;
    public byte Drawer {
      get { return _drawer; }
      set {
        if (value > 4)
          value = 4;
        if (_drawer == 4) {
          Visible = value != 4;
        }
        if (value != _drawer) {
          _drawer = value;
          OnPropertyChanged();
          State = StateManager.NewState(State);
        }
      }
    }

    public bool ShouldSerializeDrawer() {
      return StateManager.GetState(State, "Drawer") > 0;
    }

    private string _language1Comment;
    public string Language1Comment {
      get { return _language1Comment; }
      set {
        if (_language1Comment == value)
          return;
        _language1Comment = value;
        OnPropertyChanged();
        State = StateManager.NewState(State);
      }
    }

    public bool ShouldSerializeLanguage1Comment() {
      return StateManager.GetState(State, "Language1Comment") > 0;
    }

    private string _language2Comment;
    public string Language2Comment {
      get { return _language2Comment; }
      set {
        if (_language2Comment == value)
          return;
        _language2Comment = value;
        OnPropertyChanged();
        State = StateManager.NewState(State);
      }
    }

    public bool ShouldSerializeLanguage2Comment() {
      return StateManager.GetState(State, "Language2Comment") > 0;
    }

    private bool _visible;
    public bool Visible {
      get { return _visible; }
      set {
        if (_visible == value)
          return;
        _visible = value;
        OnPropertyChanged();
        State = StateManager.NewState(State);
      }
    }

    public bool ShouldSerializeVisible() {
      return StateManager.GetState(State, "Visible") > 0;
    }

    public int State { get; set; }

    public Word() {
      Id = DateTime.Now.Ticks;
      _groupId = -1;
      _language1 = "";
      _language2 = "";
      _drawer = 0;
      _language1Comment = "";
      _language2Comment = "";
      _visible = true;
      State = int.MaxValue;
    }

    public override bool Equals(object obj) {
      Word word = obj as Word;
      if (word != null &&
        word.Id == Id &&
        word.GroupId == GroupId &&
        word.Drawer == Drawer &&
        word.Visible == Visible &&
        word.Language1.Equals(Language1) &&
        word.Language2.Equals(Language2)) {
        return true;
      }
      return false;
    }

    public void SwapLanguages() {
      string temp = Language1;
      Language1 = Language2;
      Language2 = temp;

      temp = Language1Comment;
      Language1Comment = Language2Comment;
      Language2Comment = temp;
    }


    
  }

  
}