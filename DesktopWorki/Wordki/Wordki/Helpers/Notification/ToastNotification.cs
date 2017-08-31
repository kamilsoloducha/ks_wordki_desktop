using Controls.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.Notification
{
    public class ToastNotification : INotification
    {
        public void Show(string message)
        {
            Toaster.NewToast(new ToastProperties { Message = message });
        }
    }
}
