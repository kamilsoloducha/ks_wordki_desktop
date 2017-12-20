using System;
using System.Windows.Input;

namespace Wordki.ViewModels.Dialogs
{
    public abstract class DialogViewModelBase : ViewModelBase
    {
        public event EventHandler ClosingRequest;

        public ICommand CloseCommand { get; protected set; }

        public Action CloseAction { get; set; }

        public override void Back()
        {
        }

        public override void InitViewModel(object parameter = null)
        {
            CloseCommand = new Util.BuilderCommand(Close);
        }

        public override void Loaded()
        {

        }

        public override void Unloaded()
        {

        }

        protected void OnClosingRequest()
        {
            if (ClosingRequest != null)
            {
                ClosingRequest(this, EventArgs.Empty);
            }
        }

        protected virtual void Close()
        {
            if (CloseAction != null)
            {
                CloseAction.Invoke();
            }
            if (ClosingRequest != null)
            {
                ClosingRequest(this, EventArgs.Empty);
            }
        }

    }
}
