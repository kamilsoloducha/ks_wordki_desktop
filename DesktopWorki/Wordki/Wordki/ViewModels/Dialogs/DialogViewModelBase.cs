using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override void InitViewModel()
        {
            CloseCommand = new Util.BuilderCommand(Close);
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
            if(ClosingRequest != null)
            {
                ClosingRequest(this, EventArgs.Empty);
            }
        }

    }
}
