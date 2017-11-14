using Controls.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wordki.InteractionProvider.InfoProvider
{
    public class ToastProvider : IInfoProvider
    {

        public string Message { get; set; }

        public void Interact()
        {
            if (App.Current.Dispatcher.CheckAccess())
            {
                DispatcherWork();
            }
            else
            {
                App.Current.Dispatcher.Invoke(DispatcherWork);
            }
        }

        private void DispatcherWork()
        {
            Toaster.NewToast(new ToastProperties()
            {
                Message = Message;
            });
        }
    }
}
