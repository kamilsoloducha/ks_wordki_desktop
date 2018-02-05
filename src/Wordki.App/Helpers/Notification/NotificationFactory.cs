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
