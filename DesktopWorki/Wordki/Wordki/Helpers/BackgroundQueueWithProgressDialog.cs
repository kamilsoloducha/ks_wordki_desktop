using System.ComponentModel;
using System.Windows;
using Util.Threads;
using Wordki.Views.Dialogs.Progress;

namespace Wordki.Helpers
{
    public class BackgroundQueueWithProgressDialog : BackgroundWorkQueue
    {

        ProcessDialog dialog;

        public BackgroundQueueWithProgressDialog() : base()
        {
            dialog = new ProcessDialog()
            {
                DialogTitle = "test",
                ButtonLabel = "Cancel",
                OnCanceled = () => Abort(),
            };
        }

        public override void Execute()
        {
            base.Execute();
            dialog.Show();
        }

        protected override void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            base.worker_DoWork(sender, e);
            Application.Current.Dispatcher.Invoke(() => dialog.Close());
        }

        public override void Abort()
        {
            base.Abort();
            dialog.Close();
        }

    }
}
