using System;

namespace Wordki.Views.Dialogs.Progress
{
    public interface IProgressNotification
    {
        string Title { get; set; }
        string Message { get; set; }
        string ButtonLabel { get; set; }
        bool CanCanceled { get; set; }
        Action OnCanceled { get; set; }
        void Show();
        void Close();

    }
}
