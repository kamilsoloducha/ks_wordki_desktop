using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wordki.Helpers;

namespace Wordki.ViewModels.Dialogs.Progress
{
    public class ProgressDialogViewModel : ViewModelBase
    {

        public event EventHandler ClosingRequest;

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

        public ProgressDialogViewModel()
        {
            CancelCommand = new Helpers.BuilderCommand(Cancel);
        }

        public override void Back()
        {
        }

        public override void InitViewModel()
        {
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

        protected void OnClosingRequest()
        {
            if (ClosingRequest != null)
            {
                ClosingRequest(this, EventArgs.Empty);
            }
        }
    }
}
