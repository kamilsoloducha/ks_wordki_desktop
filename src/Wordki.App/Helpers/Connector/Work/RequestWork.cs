using Newtonsoft.Json;
using Oazachaosu.Core.Common;
using System;
using Util.Threads;
using Wordki.Helpers.Connector.SimpleConnector;

namespace Wordki.Helpers.Connector.Work
{

    public class ApiWork<T> : IWork
    {
        private ApiConnector connector;

        public Func<bool> InitializeFunc { get; set; }
        public Action OnCanceledFunc { get; set; }
        public Action<T> OnCompletedFunc { get; set; }
        public Action<ErrorDTO> OnFailedFunc { get; set; }

        public IRequest Request { get; set; }
        public T Object { get; private set; }

        public ApiWork()
        {
            connector = new ApiConnector();
        }

        public virtual bool Execute()
        {
            try
            {
                string messageBack = connector.SendRequest(Request);
                Object = JsonConvert.DeserializeObject<T>(messageBack);
            }
            catch (Exception)
            {
                return false;
            }
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
            OnCompletedFunc.Invoke(Object);
        }

        public void OnFailed()
        {
            if (OnFailedFunc == null)
            {
                return;
            }
            OnFailedFunc.Invoke();
        }

    }
}
