using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordki.Views.Dialogs.Progress
{
    public class ProgressNotificationFactory
    {
        public static IProgressNotification CreateProgresNotification(ProgressNotificationType type)
        {
            switch (type)
            {
                case ProgressNotificationType.ProgressDialog: return new ProcessDialog();
                default: throw new ArgumentOutOfRangeException($"Wrong value of ProgressNotificationType: {type}");

            }
        }
    }
}
