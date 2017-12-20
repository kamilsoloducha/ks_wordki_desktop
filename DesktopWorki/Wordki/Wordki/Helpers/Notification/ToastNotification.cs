using Controls.Notification;

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
