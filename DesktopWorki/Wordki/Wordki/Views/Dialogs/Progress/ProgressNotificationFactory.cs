using System;
using System.Windows;
using Util.Threads;

namespace Wordki.Views.Dialogs.Progress
{
    public class ProgressNotificationFactory
    {
        public static IProgressDialog CreateProgresNotification(ProgressNotificationType type)
        {
            switch (type)
            {
                case ProgressNotificationType.ProgressDialog:
                    {
                        ProgressDialog dialog = new ProgressDialog();
                        dialog.ViewModel = new ViewModels.Dialogs.Progress.ProgressDialogViewModel();
                        return dialog;
                    }
                default: throw new ArgumentOutOfRangeException($"Wrong value of ProgressNotificationType: {type}");

            }
        }
    }
}
