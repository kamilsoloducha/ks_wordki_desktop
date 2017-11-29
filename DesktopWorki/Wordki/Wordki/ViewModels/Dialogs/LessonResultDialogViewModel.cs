using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Wordki.InteractionProvider;
using System.Globalization;
using System.Collections;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Util.Collections;

namespace Wordki.ViewModels.Dialogs
{
    public class LessonResultDialogViewModel : DialogViewModelBase
    {

        private IResult selectedItem;
        public IResult SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem == value)
                {
                    return;
                }
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        public ICommand PreviousCommand { get; }
        public ICommand NextCommand { get; }

        public IList<IResult> Results { get; set; }

        public LessonResultDialogViewModel(ILessonResultProvider model) : base()
        {
            CloseAction = model.OnClose;
            Results = model.Results.OrderBy(x => x.Group.CreationDate).ToList();
            PreviousCommand = new Util.BuilderCommand(Previous);
            NextCommand = new Util.BuilderCommand(Next);
            SelectedItem = Results[0];
        }

        private void Next()
        {
            IResult previous = Results.Next(SelectedItem);
            if (previous != null)
            {
                SelectedItem = previous;
            }
        }

        private void Previous()
        {
            IResult previous = Results.Previous(SelectedItem);
            if (previous != null)
            {
                SelectedItem = previous;
            }
        }
    }
}
