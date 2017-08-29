using System;
using System.Collections.Generic;
using System.Threading;
using Wordki.Views.Dialogs;

namespace Wordki.Helpers.Command
{
    public class CommandQueue<T> where T : ICommand
    {

        public LinkedList<T> MainQueue { get; private set; }
        public Action<bool> OnQueueComplete { get; set; }
        public bool CreateDialog { get; set; }

        private Thread Thread { get; set; }
        private ProcessDialog ProcessDialog { get; set; }
        private volatile bool _isRunning;

        public CommandQueue()
        {
            MainQueue = new LinkedList<T>();
            CreateDialog = true;
        }

        public void Execute()
        {
            Thread = new Thread(() => {
                while (MainQueue.Count != 0)
                {
                    T lCommandToExecute = MainQueue.First.Value;
                    bool isCorrect = lCommandToExecute.Execute();
                    if (!_isRunning || !isCorrect)
                    {
                        if (OnQueueComplete != null)
                        {
                            OnQueueComplete(false);
                        }
                        HideDialog();
                        return;
                    }
                    MainQueue.RemoveFirst();
                }
                if (OnQueueComplete != null)
                {
                    OnQueueComplete(true);
                }
                _isRunning = false;
                HideDialog();
            });
            _isRunning = true;
            Thread.Start();
            ShowDialog();
        }

        private void ShowDialog()
        {
            if (CreateDialog)
            {
                ProcessDialog = new ProcessDialog();
                ProcessDialog.DialogTitle = "Uwaga";
                ProcessDialog.Message = "Trwa przetwarzanie...";
                ProcessDialog.CancelLabel = "Anuluj";
                ProcessDialog.CancelCommand = new BuilderCommand(delegate {
                    _isRunning = false;
                    Thread.Abort();
                    if (ProcessDialog.IsVisible)
                    {
                        ProcessDialog.Close();
                    }
                    if (OnQueueComplete != null)
                    {
                        OnQueueComplete(false);
                    }
                });
                try
                {
                    if (_isRunning)
                    {
                        ProcessDialog.ShowDialog();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        private void HideDialog()
        {
            if (ProcessDialog != null && Thread.IsAlive)
            {
                try
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((() => ProcessDialog.Close()));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
