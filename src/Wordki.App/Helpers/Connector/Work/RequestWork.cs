using Newtonsoft.Json;
using NLog;
using Oazachaosu.Core.Common;
using System;
using Util.Threads;
using Wordki.Helpers.Connector.SimpleConnector;

namespace Wordki.Helpers.Connector.Work
{

    public class ApiWork<T> : IWork
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
                logger.Info($"Wysyła request; '{Request.Url}'\n" +
                    $"Message: '{Request.Message}'\n" +
                    $"Method: '{Request.Method}'");
                message = connector.SendRequest(Request);
                logger.Info($"Odpowiedz: '{message}'");
            }
            catch (Exception e)
            {
                logger.Error($"Exception was thrown.\n" +
                    $"Message: '{e.Message}'\n" +
                    $"Type: '{e.GetType()}'\n" +
                    $"StackTrace: '{e.StackTrace}'");
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
            if (error == null)
            {
                error = new ErrorDTO
                {
                    Code = ErrorCode.Undefined,
                    Message = "Error message does not contain any usefull information"
                };
            }
            OnFailedFunc.Invoke(error);
        }

    }
}
