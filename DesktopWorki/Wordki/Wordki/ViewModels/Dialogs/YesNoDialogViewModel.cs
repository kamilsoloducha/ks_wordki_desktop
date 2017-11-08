using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Util;

namespace Wordki.ViewModels.Dialogs
{
    public class YesNoDialogViewModel : DialogViewModelBase
    {

        public ICommand PositiveCommand { get; set; }
        public ICommand NegativeCommand { get; set; }

        public Action NoAction { get; set; }
        public Action YesAction { get; set; }


        #region Properties

        private string _dialogTitle;
        public string DialogTitle
        {
            get { return _dialogTitle; }
            set
            {
                if (_dialogTitle == value)
                {
                    return;
                }
                _dialogTitle = value;
                OnPropertyChanged();
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message == value)
                {
                    return;
                }
                _message = value;
                OnPropertyChanged();
            }
        }
        private string _positiveLabel;

        public string PositiveLabel
        {
            get { return _positiveLabel; }
            set
            {
                if (_positiveLabel == value)
                {
                    return;
                }
                _positiveLabel = value;
                OnPropertyChanged();
            }
        }
        private string _negativeLabel;

        public string NegativeLabel
        {
            get { return _negativeLabel; }
            set
            {
                if (_negativeLabel == value)
                {
                    return;
                }
                _negativeLabel = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public YesNoDialogViewModel()
        {

        }

        public override void InitViewModel()
        {
            PositiveCommand = new BuilderCommand(Positive);
            NegativeCommand = new BuilderCommand(Negative);
        }

        private void Negative(object obj)
        {
            if (NoAction != null)
            {
                NoAction.Invoke();
            }
            OnClosingRequest();
        }

        private void Positive(object obj)
        {
            if (YesAction != null)
            {
                YesAction.Invoke();
            }
            OnClosingRequest();
        }
    }
}