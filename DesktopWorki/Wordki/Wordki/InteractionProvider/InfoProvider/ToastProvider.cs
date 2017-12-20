using Controls.Notification;

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
