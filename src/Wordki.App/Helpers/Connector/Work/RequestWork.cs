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
        private string message;

        public Func<bool> InitializeFunc { get; set; }
        public Action OnCanceledFunc { get; set; }
        public Action<T> OnCompletedFunc { get; set; }
        public Action<ErrorDTO> OnFailedFunc { get; set; }

        public IRequest Request { get; set; }

        public ApiWork()
        {
            connector = new ApiConnector();
        }

        public virtual bool Execute()
        {
            try
            {
                Console.WriteLine($"Wysyła request; '{Request.Url}'");
                Console.WriteLine($"Message: '{Request.Message}'");
                Console.WriteLine($"Method: '{Request.Method}'");
                message = connector.SendRequest(Request);
                Console.WriteLine($"Odpowiedz: '{message}'");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception was thrown.\n" +
                    $"Message: '{e.Message}'\n" +
                    $"Type: '{e.GetType()}'\n" +
                    $"StackTrace: '{e.StackTrace}'");
                return false;
            }
            return connector.IsSuccess;
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
            T obj = JsonConvert.DeserializeObject<T>(message);
            OnCompletedFunc.Invoke(obj);
        }

        public void OnFailed()
        {
            if (OnFailedFunc == null)
            {
                return;
            }
            ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(message);
            OnFailedFunc.Invoke(error);
        }

    }
}
