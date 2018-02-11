using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Oazachaosu.Core.Common;
using WordkiModel;

namespace Wordki.Models
{
    [Serializable]
    public class Group : ModelBase<IGroup>, IComparable<IGroup>, IGroup
    {

        public virtual long Id { get; set; }

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

        private DateTime creationDate;
        public virtual DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                if (creationDate == value)
                {
                    return;
                }
                creationDate = value;
                OnPropertyChanged();
            }
        }


        public virtual bool ShouldSerializeLanguage2()
        {
            return StateManager.GetState(State, "Language2") > 0;
        }

        public virtual int State { get; set; }

        private IList<IWord> words;
        public virtual IList<IWord> Words
        {
            get { return words; }
            set
            {
                if (!(value is ObservableCollection<IWord>))
                {
                    value = new ObservableCollection<IWord>(value);
                }
                words = value;
            }
        }

        private IList<IResult> results;
        public virtual IList<IResult> Results
        {
            get { return results; }
            set
            {
                if (!(value is ObservableCollection<IResult>))
                {
                    value = new ObservableCollection<IResult>(value);
                }
                results = value;
            }
        }

        public Group()
        {
            Id = DateTime.Now.Ticks;
            _name = "";
            _language1 = LanguageType.Default;
            _language2 = LanguageType.Default;
            State = int.MaxValue;
            CreationDate = DateTime.Now;
            Words = new ObservableCollection<IWord>();
            Results = new ObservableCollection<IResult>();
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
