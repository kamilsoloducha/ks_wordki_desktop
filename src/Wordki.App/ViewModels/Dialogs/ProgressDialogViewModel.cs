using System;
using Wordki.Helpers;

namespace Wordki.ViewModels.Dialogs.Progress
{
    public class ProgressDialogViewModel : DialogViewModelBase
    {

        private string _dialogTitle;
        public string DialogTitle
        {
            get { return _dialogTitle; }
            set
            {
                if (_dialogTitle != value)
                {
                    _dialogTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _cancelLabel;
        public string ButtonLabel
        {
            get { return _cancelLabel; }
            set
            {
                if (_cancelLabel != value)
                {
                    _cancelLabel = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _canCanceled;
        public bool CanCanceled
        {
            get { return _canCanceled; }
            set
            {
                if (_canCanceled == value)
                {
                    return;
                }
                _canCanceled = value;
                OnPropertyChanged();
            }
        }
        public System.Windows.Input.ICommand CancelCommand { get; private set; }

        public Action OnCanceled { get; set; }

        public ProgressDialogViewModel() : base()
        {
            CancelCommand = new Util.BuilderCommand(Cancel);
        }

        private void Cancel(object obj)
        {
            if (OnCanceled != null)
            {
                OnCanceled();
            }
            OnClosingRequest();
            LoggerSingleton.LogInfo("Cancel");
        }

    }
}
