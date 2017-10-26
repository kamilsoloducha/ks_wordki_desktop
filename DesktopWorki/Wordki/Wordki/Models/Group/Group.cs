using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Repository.Models;
using Repository.Models.Language;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Wordki.Models
{
    [Serializable]
    public class Group : ModelBase<IGroup>, IComparable<IGroup>, IGroup
    {

        public virtual long Id { get; set; }

        public virtual long UserId { get; set; }

        private string _name;
        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;
                _name = value;
                State = StateManager.NewState(State);
                OnPropertyChanged();
            }
        }

        public virtual bool ShouldSerializeName()
        {
            return StateManager.GetState(State, "Name") > 0;
        }

        private LanguageType _language1;
        public virtual LanguageType Language1
        {
            get { return _language1; }
            set
            {
                if (_language1 == value)
                    return;
                _language1 = value;
                State = StateManager.NewState(State);
                OnPropertyChanged();
            }
        }

        public virtual bool ShouldSerializeLanguage1()
        {
            return StateManager.GetState(State, "Language1") > 0;
        }

        private LanguageType _language2;
        public virtual LanguageType Language2
        {
            get { return _language2; }
            set
            {
                if (_language2 == value)
                    return;
                _language2 = value;
                State = StateManager.NewState(State);
                OnPropertyChanged();
            }
        }

        public virtual bool ShouldSerializeLanguage2()
        {
            return StateManager.GetState(State, "Language2") > 0;
        }

        public virtual int State { get; set; }

        [JsonIgnore]
        public virtual IList<IWord> Words { get; set; }

        [JsonIgnore]
        public virtual IList<IResult> Results { get; set; }


        public Group()
        {
            Id = DateTime.Now.Ticks;
            _name = "";
            _language1 = LanguageType.Default;
            _language2 = LanguageType.Default;
            State = int.MaxValue;
            Words = new List<IWord>();
            Results = new List<IResult>();
        }

        public virtual int CompareTo(IGroup other)
        {
            return String.Compare(Name.ToLower(), other.Name.ToLower(), StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            IGroup group = obj as IGroup;
            return group != null &&
                   group.Id == Id &&
                   group.Name.Equals(Name) &&
                   group.Language1 == Language1 &&
                   group.Language2 == Language2;
        }

        public virtual IResult GetLastResult()
        {
            return Results.OrderBy(x => x.DateTime).FirstOrDefault();
        }

        public virtual void AddWord(IWord word)
        {
            word.Group = this;
            Words.Add(word);
        }

        public virtual void AddResult(IResult result)
        {
            result.Group = this;
            Results.Add(result);
        }
    }
}
