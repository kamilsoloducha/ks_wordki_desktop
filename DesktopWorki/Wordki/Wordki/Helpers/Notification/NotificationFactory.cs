using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Helpers.Notification
{
    public static class NotificationFactory
    {
        public static INotification Create()
        {
            return new ToastNotification();
        }
    }
}
