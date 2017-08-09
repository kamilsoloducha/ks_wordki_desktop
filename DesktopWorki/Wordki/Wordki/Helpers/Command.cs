using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Wordki.Models.Connector;
using Wordki.Views.Dialogs;

namespace Wordki.Helpers {

  public interface ICommand {
    bool Execute();
  }

  public class CommandApiRequest : ICommand {
    public delegate void OnCompleteApiRequestDelegate(ApiResponse pResponse);

    public OnCompleteApiRequestDelegate OnCompleteCommand { get; set; }
    private ApiRequest Request { get; set; }

    public CommandApiRequest(ApiRequest pRequest) {
      Request = pRequest;
    }

    public bool Execute() {
      Request.PrepareMessage();
      ApiConnector lConnector = new ApiConnector();
      ApiResponse lResponse = lConnector.SentRequest(Request);
      if (OnCompleteCommand != null && lResponse != null) {
        OnCompleteCommand(lResponse);
      }
      return lResponse != null && !lResponse.IsError;
    }
  }

  public delegate Task<bool> SimpeCommandAsyncDelegate();
  public class SimpleCommandAsync : ICommand {

    private SimpeCommandAsyncDelegate Action { get; set; }

    public SimpleCommandAsync(SimpeCommandAsyncDelegate pAction) {
      Action = pAction;
    }

    public bool Execute() {
      try {
        Action.Invoke();
      } catch (Exception) {
        return false;
      }
      return true;
    }
  }


  public delegate bool SimpleCommandDelegate();
  public class SimpleCommand : ICommand {

    private SimpleCommandDelegate Action { get; set; }

    public SimpleCommand(SimpleCommandDelegate pAction) {
      Action = pAction;
    }

    public bool Execute() {
      try {
        Action.Invoke();
      } catch (Exception) {
        return false;
      }
      return true;
    }
  }

  public delegate void OnQueueCompleteDelegate(bool pSuccess);
  public class CommandQueue<T> where T : ICommand {

    public LinkedList<T> MainQueue { get; private set; }
    public OnQueueCompleteDelegate OnQueueComplete { get; set; }
    public bool CreateDialog { get; set; }

    private Thread Thread { get; set; }
    private ProcessDialog ProcessDialog { get; set; }
    private volatile bool _isRunning;

    public CommandQueue() {
      MainQueue = new LinkedList<T>();
      CreateDialog = true;
    }

    public void Execute() {
      Thread = new Thread(() => {
        while (MainQueue.Count != 0) {
          T lCommandToExecute = MainQueue.First.Value;
          bool isCorrect = lCommandToExecute.Execute();
          if (!_isRunning || !isCorrect) {
            if (OnQueueComplete != null) {
              OnQueueComplete(false);
            }
            HideDialog();
            return;
          }
          MainQueue.RemoveFirst();
        }
        if (OnQueueComplete != null) {
          OnQueueComplete(true);
        }
        _isRunning = false;
        HideDialog();
      });
      _isRunning = true;
      Thread.Start();
      ShowDialog();
    }

    private void ShowDialog() {
      if (CreateDialog) {
        ProcessDialog = new ProcessDialog();
        ProcessDialog.DialogTitle = "Uwaga";
        ProcessDialog.Message = "Trwa przetwarzanie...";
        ProcessDialog.CancelLabel = "Anuluj";
        ProcessDialog.CancelCommand = new BuilderCommand(delegate {
          _isRunning = false;
          Thread.Abort();
          if (ProcessDialog.IsVisible) {
            ProcessDialog.Close();
          }
          if (OnQueueComplete != null) {
            OnQueueComplete(false);
          }
        });
        try {
          if (_isRunning) {
            ProcessDialog.ShowDialog();
          }
        } catch (Exception e) {
          Console.WriteLine(e.StackTrace);
        }
      }
    }

    private void HideDialog() {
      if (ProcessDialog != null && Thread.IsAlive) {
        try {
          Application.Current.Dispatcher.Invoke((() => ProcessDialog.Close()));
        } catch (Exception e) {
          Console.WriteLine(e.Message);
        }
      }
    }
  }
}
