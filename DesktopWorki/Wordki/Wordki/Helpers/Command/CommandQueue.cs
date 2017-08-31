using System;
using System.Collections.Generic;
using System.Threading;
using Wordki.Views.Dialogs.Progress;

namespace Wordki.Helpers.Command
{
    public class CommandQueue<T> where T : ICommand
    {

        public LinkedList<T> MainQueue { get; private set; }
        public Action<bool> OnQueueComplete { get; set; }
        public bool CreateDialog { get; set; }

        private Thread Thread { get; set; }
        private IProgressNotification ProcessDialog { get; set; }
        private volatile bool _isRunning;

        public CommandQueue()
        {
            MainQueue = new LinkedList<T>();
            CreateDialog = true;
        }

        public void Execute()
        {
            Thread = new Thread(() =>
            {
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
                ProcessDialog = ProgressNotificationFactory.CreateProgresNotification(ProgressNotificationType.ProgressDialog);
                ProcessDialog.Title = "Uwaga";
                ProcessDialog.Message = "Trwa przetwarzanie...";
                ProcessDialog.ButtonLabel = "Anuluj";
                ProcessDialog.OnCanceled += Cancel;
                try
                {
                    if (_isRunning)
                    {
                        ProcessDialog.Show();
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

        private void Cancel()
        {
            _isRunning = false;
            Thread.Abort();
            if (OnQueueComplete != null)
            {
                OnQueueComplete(false);
            }
        }
    }
}
