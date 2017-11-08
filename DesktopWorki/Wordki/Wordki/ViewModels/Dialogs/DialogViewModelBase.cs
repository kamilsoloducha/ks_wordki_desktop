using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.ViewModels.Dialogs
{
    public abstract class DialogViewModelBase : ViewModelBase
    {
        public override void Back()
        {
        }

        public event EventHandler ClosingRequest;

        protected void OnClosingRequest()
        {
            if (ClosingRequest != null)
            {
                ClosingRequest(this, EventArgs.Empty);
            }
        }
    }
}
