using System.Windows;

namespace Wordki.InteractionProvider
{
    public abstract class InteractionProviderBase : IInteractionProvider
    {

        public void Interact()
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                DispatcherWork();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(DispatcherWork);
            }
        }

        protected abstract void DispatcherWork();

    }
}
