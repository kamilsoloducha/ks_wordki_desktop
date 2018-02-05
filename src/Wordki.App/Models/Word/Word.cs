using System;
using Newtonsoft.Json;
using WordkiModel;

namespace Wordki.Models
{
    [Serializable]
    public class Word : ModelBase<IWord>, IWord
    {
        private static int _maxWordLength = 512;

        public virtual long Id { get; set; }

        [JsonIgnore]
        public virtual long UserId { get; set; }

        private IGroup _group;
        public virtual IGroup Group
        {
            get { return _group; }
            set
            {
                if (value == _group)
                {
                    return;
                }
                _group = value;
            }
        }

        private string _language1;
        public virtual string Language1
        {
            get { return _language1; }
            set
            {
                if (_language1 == value)
                    return;
                if (value.Length > _maxWordLength)
                {
                    value = value.Remove(_maxWordLength - 1);
                }
                _language1 = value;
                OnPropertyChanged();
                State = StateManager.NewState(State);
            }
        }

        public virtual bool ShouldSerializeLanguage1()
        {
            return StateManager.GetState(State, "Language1") > 0;
        }

        private string _language2;
        public virtual string Language2
        {
            get { return _language2; }
            set
            {
                if (_language2 == value)
                    return;
                if (value.Length > _maxWordLength)
                {
                    value = value.Remove(_maxWordLength - 1);
                }
                _language2 = value;
                OnPropertyChanged();
                State = StateManager.NewState(State);
            }
        }

        public virtual bool ShouldSerializeLanguage2()
        {
            return StateManager.GetState(State, "Language2") > 0;
        }


        private byte _drawer;
        public virtual byte Drawer
        {
            get { return _drawer; }
            set
            {
                if (value > 4)
                    value = 4;
                if (_drawer == 4)
                {
                    IsVisible = value != 4;
                }
                if (value != _drawer)
                {
                    _drawer = value;
                    OnPropertyChanged();
                    State = StateManager.NewState(State);
                }
            }
        }

        public virtual bool ShouldSerializeDrawer()
        {
            return StateManager.GetState(State, "Drawer") > 0;
        }

        private string _language1Comment;
        public virtual string Language1Comment
        {
            get { return _language1Comment; }
            set
            {
                if (_language1Comment == value)
                    return;
                _language1Comment = value;
                OnPropertyChanged();
                State = StateManager.NewState(State);
            }
        }

        public virtual bool ShouldSerializeLanguage1Comment()
        {
            return StateManager.GetState(State, "Language1Comment") > 0;
        }

        private string _language2Comment;
        public virtual string Language2Comment
        {
            get { return _language2Comment; }
            set
            {
                if (_language2Comment == value)
                    return;
                _language2Comment = value;
                OnPropertyChanged();
                State = StateManager.NewState(State);
            }
        }

        public virtual bool ShouldSerializeLanguage2Comment()
        {
            return StateManager.GetState(State, "Language2Comment") > 0;
        }

        private bool _visible;
        public virtual bool IsVisible
        {
            get { return _visible; }
            set
            {
                if (_visible == value)
                    return;
                _visible = value;
                OnPropertyChanged();
                State = StateManager.NewState(State);
            }
        }

        public virtual bool ShouldSerializeVisible()
        {
            return StateManager.GetState(State, "Visible") > 0;
        }

        public virtual int State { get; set; }


        private bool selected;
        public virtual bool IsSelected
        {
            get { return selected; }
            set
            {
                if (selected == value)
                {
                    return;
                }
                selected = value;
                OnPropertyChanged();
                State = StateManager.NewState(State);
            }
        }


        public virtual bool ShouldSerializeSelected()
        {
            return StateManager.GetState(State, "Selected") > 0;
        }

        private ushort _repeatingNumber;
        public virtual ushort RepeatingCounter
        {
            get { return _repeatingNumber; }
            set
            {
                if (_repeatingNumber == value)
                {
                    return;
                }
                _repeatingNumber = value;
                OnPropertyChanged();
                State = StateManager.NewState(State);
            }
        }

        public virtual bool ShouldSerializeRepeatingNumber()
        {
            return StateManager.GetState(State, "RepeatingNumber") > 0;
        }

        private DateTime _lastRepeating;

        public virtual DateTime LastRepeating
        {
            get { return _lastRepeating; }
            set
            {
                if (_lastRepeating == value)
                {
                    return;
                }
                _lastRepeating = value;
                OnPropertyChanged();
                State = StateManager.NewState(State);
            }
        }

        public virtual bool ShouldSerializeLastRepeating()
        {
            return StateManager.GetState(State, "LastRepeating") > 0;
        }

        private string comment;
        public virtual string Comment
        {
            get { return comment; }
            set
            {
                if (comment == value)
                {
                    return;
                }
                comment = value;
                OnPropertyChanged();
                State = StateManager.NewState(State);
            }
        }

        public virtual bool ShouldSerializeComment()
        {
            return StateManager.GetState(State, "Comment") > 0;
        }


        public Word()
        {
            Id = DateTime.Now.Ticks;
            Group = FakeGroup.Group;
            _language1 = "";
            _language2 = "";
            _drawer = 0;
            _language1Comment = "";
            _language2Comment = "";
            _visible = true;
            selected = false;
            _repeatingNumber = 0;
            State = int.MaxValue;
        }

        public override bool Equals(object obj)
        {
            IWord word = obj as IWord;
            return word != null
                && word.Id == Id
                && word.Drawer == Drawer
                && word.IsVisible == IsVisible
                && word.Language1.Equals(Language1)
                && word.Language2.Equals(Language2)
                && word.IsVisible == IsVisible
                && word.IsSelected == IsSelected;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }


}