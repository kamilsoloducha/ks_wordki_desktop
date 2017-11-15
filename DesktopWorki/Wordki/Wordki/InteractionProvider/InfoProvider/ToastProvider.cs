using Controls.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wordki.InteractionProvider
{
    public class ToastProvider : InteractionProviderBase, IInfoProvider
    {

        public string Message { get; set; }

        protected override void DispatcherWork()
        {
            Toaster.NewToast(new ToastProperties()
            {
                Message = Message,
            });
        }
    }
}
