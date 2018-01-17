using System;
using Util.Threads;

namespace Wordki.Helpers.Connector.Work
{
    public class RequestWork<T> : IWork
    {
        public Func<bool> InitializeFunc { get; set; }
        public Action OnCanceledFunc { get; set; }
        public Action<T> OnCompletedFunc { get; set; }
        public Action<T> OnFailedFunc { get; set; }

        IConnector<T> Connector { get; set; }
        IRequest Request { get; set; }
        T Response { get; set; }

        public virtual bool Execute()
        {
            Response = Connector.SendRequest(Request);
            return true;
        }

        public bool Initialize()
        {
            if (InitializeFunc == null)
            {
                return true;
            }
            return InitializeFunc.Invoke();
        }

        public void OnCanceled()
        {
            if (OnCanceledFunc == null)
            {
                return;
            }
            OnCanceledFunc.Invoke();
        }

        public void OnCompleted()
        {
            if (OnCompletedFunc == null)
            {
                return;
            }
            OnCompletedFunc.Invoke(Response);
        }

        public void OnFailed()
        {
            if (OnFailedFunc == null)
            {
                return;
            }
            OnFailedFunc.Invoke(Response);
        }

    }
}
