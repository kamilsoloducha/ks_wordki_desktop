﻿using System;
using Newtonsoft.Json;
using Repository.Models;

namespace Wordki.Models
{
    [Serializable]
    public class Word : ModelAbs<IWord>, IWord
    {
        private static int _maxWordLength = 256;

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
                _group.AddWord(this);
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
                    Visible = value != 4;
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
        public virtual bool Visible
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
            State = int.MaxValue;
        }

        public override bool Equals(object obj)
        {
            IWord word = obj as IWord;
            if (word != null &&
              word.Id == Id &&
              word.Drawer == Drawer &&
              word.Visible == Visible &&
              word.Language1.Equals(Language1) &&
              word.Language2.Equals(Language2))
            {
                return true;
            }
            return false;
        }

    }


}