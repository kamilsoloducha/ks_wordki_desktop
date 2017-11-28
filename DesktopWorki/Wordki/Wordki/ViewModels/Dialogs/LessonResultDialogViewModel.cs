using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Wordki.InteractionProvider;
using System.Globalization;
using System.Collections;
using System.Windows.Input;

namespace Wordki.ViewModels.Dialogs
{
    public class LessonResultDialogViewModel : DialogViewModelBase
    {

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (selectedIndex == value)
                {
                    return;
                }
                selectedIndex = value;
                OnPropertyChanged();
            }
        }

        public ICommand PreviousCommand { get; }
        public ICommand NextCommand { get; }

        internal IList<IResult> Results { get; set; }

        public LessonResultDialogViewModel(ILessonResultProvider model) : base()
        {
            CloseAction = model.OnClose;
            Results = model.Results.ToList();
            PreviousCommand = new Util.BuilderCommand(Previous);
            NextCommand = new Util.BuilderCommand(Next);
        }

        private void Next()
        {
            if (SelectedIndex < Results.Count - 1)
            {
                SelectedIndex++;
            }
        }

        private void Previous()
        {
            if (SelectedIndex > 0)
            {
                SelectedIndex--;
            }
        }
    }

    public class CollectionIndexToIndexerEnabledConverter : IValueConverter
    {
        public CheckingElement Index { get; set; }

        private IList collection;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (collection == null)
            {
                collection = parameter as IList;
            }
            return collection.IndexOf(value) == GetValueToCompare();
        }

        private int GetValueToCompare()
        {
            if (Index == CheckingElement.First)
            {
                return 0;
            }
            else
            {
                return collection.Count - 1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public enum CheckingElement
    {
        First,
        Last,
    }
}
