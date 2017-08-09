using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using Repository.Models;
using Repository.Models.Language;

namespace Wordki.Models {
  public class Group : ModelAbs<IGroup>, IComparable<Group>, IGroup {

    public long Id { get; set; }

    public long UserId { get; set; }

    private string _name;
    public string Name {
      get { return _name; }
      set {
        if (_name == value)
          return;
        _name = value;
        State = StateManager.NewState(State);
        OnPropertyChanged();
      }
    }

    public bool ShouldSerializeName() {
      return StateManager.GetState(State, "Name") > 0;
    }

    private ILanguage _language1;
    public ILanguage Language1 {
      get { return _language1; }
      set {
        if (_language1 == value)
          return;
        _language1 = value;
        State = StateManager.NewState(State);
        OnPropertyChanged();
      }
    }

    public bool ShouldSerializeLanguage1() {
      return StateManager.GetState(State, "Language1") > 0;
    }

    private ILanguage _language2;
    public ILanguage Language2 {
      get { return _language2; }
      set {
        if (_language2 == value)
          return;
        _language2 = value;
        State = StateManager.NewState(State);
        OnPropertyChanged();
      }
    }

    public bool ShouldSerializeLanguage2() {
      return StateManager.GetState(State, "Language2") > 0;
    }

    public int State { get; set; }

    [JsonIgnore]
    public ObservableCollection<Word> WordsList { get; set; }
    [JsonIgnore]
    public ObservableCollection<Result> ResultsList { get; set; }

    public Group() {
      Id = DateTime.Now.Ticks;
      _name = "";
      _language1 = LanguageFactory.GetLanguage(LanguageType.Default);
      _language2 = LanguageFactory.GetLanguage(LanguageType.Default);
      State = int.MaxValue;
      WordsList = new ObservableCollection<Word>();
      ResultsList = new ObservableCollection<Result>();
    }

    public int CompareTo(Group other) {
      return String.Compare(Name.ToLower(), other.Name.ToLower(), StringComparison.Ordinal);
    }

    public int GetLessonTime(DateTime pDateTime) {
      IEnumerable<Result> lDateResult = ResultsList.Where(x => pDateTime - x.DateTime < new TimeSpan(1, 0, 0));
      return lDateResult.Sum(lResult => lResult.TimeCount);
    }

    public override bool Equals(object obj) {
      Group group = obj as Group;
      return group != null &&
             group.Id == Id &&
             group.Name.Equals(Name) &&
             group.Language1 == Language1 &&
             group.Language2 == Language2;
    }

    public void SwapLanguage() {
      ILanguage temp = Language1;
      Language1 = Language2;
      Language2 = temp;
    }

    public Result GetLastResult() {
      return ResultsList.OrderBy(x => x.DateTime).FirstOrDefault();
    }
  }
}
