using Remotion.Linq.Collections;
using Repository.Models;
using Repository.Models.Enums;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using Wordki.Database;
using Wordki.Models;
using Wordki.Models.Lesson;

namespace Wordki.ViewModels.LessonStates
{
    public abstract class LessonStateBase : INotifyPropertyChanged, IDisposable
    {
        protected static IResultCalculator ResultCalculator;

        protected TranslationDirection _translationDirection;
        protected Lesson Lesson { get; set; }
        public LessonStateEnum StateEnum { get; set; }

        #region Properties
        private IWord _word;
        public IWord SelectedWord
        {
            get { return _word; }
            set
            {
                if (_word == value)
                {
                    return;
                }
                _word = value;
                OnPropertyChanged();
            }
        }

        private string _groupName;
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                if (_groupName == value) return;
                _groupName = value;
                OnPropertyChanged();
            }
        }

        private string _translation;
        public string Translation
        {
            get { return _translation; }
            set
            {
                if (_translation == value) return;
                _translation = value;
                OnPropertyChanged();
            }
        }

        private string _label;
        public string Label
        {
            get { return _label; }
            set
            {
                if (_label == value) return;
                _label = value;
                OnPropertyChanged();
            }
        }

        private string _commentLanguage;
        public string CommentLanguage
        {
            get { return _commentLanguage; }
            set
            {
                if (_commentLanguage == value) return;
                _commentLanguage = value;
                OnPropertyChanged();
            }
        }

        private string _commentTranslation;
        public string CommentTranslation
        {
            get { return _commentTranslation; }
            set
            {
                if (_commentTranslation == value) return;
                _commentTranslation = value;
                OnPropertyChanged();
            }
        }

        private bool _correctButtonEnabled;
        public bool CorrectButtonEnabled
        {
            get { return _correctButtonEnabled; }
            set
            {
                if (_correctButtonEnabled == value) return;
                _correctButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _checkButtonEnabled;
        public bool CheckButtonEnabled
        {
            get { return _checkButtonEnabled; }
            set
            {
                if (_checkButtonEnabled == value) return;
                _checkButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _wrongButtonEnabled;
        public bool WrongButtonEnabled
        {
            get { return _wrongButtonEnabled; }
            set
            {
                if (_wrongButtonEnabled == value) return;
                _wrongButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _translationBoxEnabled;
        public bool TranslationBoxEnabled
        {
            get { return _translationBoxEnabled; }
            set
            {
                if (_translationBoxEnabled == value) return;
                _translationBoxEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _translationIsFocused;
        public bool TranslationIsFocused
        {
            get { return _translationIsFocused; }
            set
            {
                if (_translationIsFocused == value) return;
                _translationIsFocused = value;
                OnPropertyChanged();
            }
        }

        private bool _wrongButtonIsFocused;
        public bool WrongButtonIsFocused
        {
            get { return _wrongButtonIsFocused; }
            set
            {
                if (_wrongButtonIsFocused == value) return;
                _wrongButtonIsFocused = value;
                OnPropertyChanged();
            }
        }

        private bool _correctButtonIsFocused;
        public bool CorrectButtonIsFocused
        {
            get { return _correctButtonIsFocused; }
            set
            {
                if (_correctButtonIsFocused == value) return;
                _correctButtonIsFocused = value;
                OnPropertyChanged();
            }
        }

        private Brush _translationColor;
        public Brush TranslationColor
        {
            get { return _translationColor; }
            set
            {
                if (_translationColor != null && _translationColor.Equals(value)) return;
                _translationColor = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<double> _drawerValues;
        public ObservableCollection<double> DrawerValues
        {
            get { return _drawerValues; }
            set
            {
                if (_drawerValues == value) return;
                _drawerValues = value;
                OnPropertyChanged();
            }
        }

        private int _maxValue;
        public int MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (_maxValue == value) return;
                _maxValue = value;
                OnPropertyChanged();
            }
        }

        private int _maxResult;
        public int MaxResult
        {
            get { return _maxResult; }
            set
            {
                if (_maxResult == value) return;
                _maxResult = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<double> _progress;
        public ObservableCollection<double> Progress
        {
            get { return _progress; }
            set
            {
                if (_progress == value) return;
                _progress = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<double> _results;
        public ObservableCollection<double> Results
        {
            get { return _results; }
            set
            {
                if (_results == value) return;
                _results = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<double> _counter;
        public ObservableCollection<double> Counter
        {
            get { return _counter; }
            set
            {
                if (_counter == value) return;
                _counter = value;
                OnPropertyChanged();
            }
        }

        private int _selectedDrawer;
        public int SelectedDrawer
        {
            get { return _selectedDrawer; }
            set
            {
                if (_selectedDrawer == value) return;
                _selectedDrawer = value;
                OnPropertyChanged();
            }
        }

        private string _startStopButtonContent;
        public string StartStopButtonContent
        {
            get { return _startStopButtonContent; }
            set
            {
                if (_startStopButtonContent == value) return;
                _startStopButtonContent = value;
                OnPropertyChanged();
            }
        }

        private ICommand _startStopButtonCommand;
        public ICommand StartStopButtonCommand
        {
            get { return _startStopButtonCommand; }
            set
            {
                if (_startStopButtonCommand == value) return;
                _startStopButtonCommand = value;
                OnPropertyChanged();
            }
        }

        #endregion

        static LessonStateBase()
        {
            ResultCalculator = new ResultCalculator();
        }

        protected LessonStateBase(Lesson pLesson)
          : this(pLesson, null) { }

        protected LessonStateBase(Lesson pLesson, LessonStateBase pLastState)
        {
            Lesson = pLesson;
            _translationDirection = UserManagerSingleton.Instence.User.TranslationDirection;
            if (pLastState != null)
            {
                _drawerValues = pLastState._drawerValues;
                _results = pLastState._results;
                _counter = pLastState._counter;
                _selectedDrawer = pLastState._selectedDrawer;
                _progress = pLastState._progress;
                _translation = pLastState._translation;
            }
            else
            {
                _drawerValues = new ObservableCollection<double> { 0, 0, 0, 0, 0 };
                _results = new ObservableCollection<double> { 0, 0, 0 };
                _progress = new ObservableCollection<double> { 0 };
                _counter = new ObservableCollection<double> { 0 };
            }
        }

        protected abstract void RefreshGroupName();
        protected abstract void RefreshTranslation();
        protected abstract void RefreshLabel();
        protected abstract void RefreshComments();
        protected abstract void RefreshCorrectButtonEnabled();
        protected abstract void RefreshCheckButtonEnabled();
        protected abstract void RefreshWrongButtonEnabled();
        protected abstract void RefreshTranslationColor();
        protected abstract void RefreshTranslationBoxEnabled();
        protected abstract void RefreshFocused();
        protected abstract void RefreshDrawerLight();
        protected abstract void RefreshStartStopButton();

        protected void RefreshDrawersValues()
        {
            if (Lesson == null) return;
            int[] values = Lesson.GetDrawerValues();
            for (int i = 0; i < values.Length; i++)
            {
                DrawerValues[i] = values[i];
            }
            MaxValue = Lesson.BeginWordsList.Count;
        }

        protected void RefreshProgress()
        {
            if (Lesson == null)
                return;
            if (Lesson.BeginWordsList.Count != 0)
                Progress[0] = Lesson.GetProgress();
        }

        protected virtual void RefreshResult()
        {
            if (Lesson.Counter > Lesson.BeginWordsList.Count)
            {
                Counter[0] = Lesson.BeginWordsList.Count;
            }
            else
            {
                Counter[0] = Lesson.Counter;
            }
            Results[2] = ResultCalculator.GetCorrectCount(Lesson.ResultList);
            Results[1] = ResultCalculator.GetAcceptedCount(Lesson.ResultList);
            Results[0] = ResultCalculator.GetWrongCount(Lesson.ResultList);
            MaxResult = (int)Results.Sum();
        }

        public virtual void RefreshView()
        {
            RefreshGroupName();
            RefreshTranslation();
            RefreshLabel();
            RefreshComments();
            RefreshCorrectButtonEnabled();
            RefreshCheckButtonEnabled();
            RefreshWrongButtonEnabled();
            RefreshTranslationColor();
            RefreshTranslationBoxEnabled();
            RefreshFocused();
            RefreshDrawersValues();
            RefreshDrawerLight();
            RefreshProgress();
            RefreshResult();
            RefreshStartStopButton();
            SelectedWord = Lesson.SelectedWord;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string pPropertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
            }
        }

        protected string GetNewLabel()
        {
            switch (_translationDirection)
            {
                case TranslationDirection.FromFirst:
                    return Lesson.SelectedWord.Language1;
                case TranslationDirection.FromSecond:
                    return Lesson.SelectedWord.Language2;
            }
            return "%ERROR%";
        }

        protected string GetTransaltion()
        {
            switch (_translationDirection)
            {
                case TranslationDirection.FromFirst:
                    return Lesson.SelectedWord.Language2;
                case TranslationDirection.FromSecond:
                    return Lesson.SelectedWord.Language1;
            }
            return "%ERROR%";
        }

        protected string GetComment()
        {
            switch (_translationDirection)
            {
                case TranslationDirection.FromFirst:
                    return Lesson.SelectedWord.Language1Comment;
                case TranslationDirection.FromSecond:
                    return Lesson.SelectedWord.Language2Comment;
            }
            return "%ERROR%";
        }

        protected string GetCommentTranslation()
        {
            switch (_translationDirection)
            {
                case TranslationDirection.FromFirst:
                    return Lesson.SelectedWord.Language2Comment;
                case TranslationDirection.FromSecond:
                    return Lesson.SelectedWord.Language1Comment;
            }
            return "%ERROR%";
        }

        public virtual void Dispose()
        {
        }
    }
}
