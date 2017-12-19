using WordkiModel;
using System;
using System.Collections.Generic;
using Wordki.ViewModels.Dialogs;
using Wordki.Views.Dialogs;

namespace Wordki.InteractionProvider
{
    public class LessonResultProvider : InteractionProviderBase, ILessonResultProvider
    {

        public IEnumerable<IResult> Results { get; set; }
        public Action OnClose { get; set; }

        protected override void DispatcherWork()
        {
            LessonResultDialog dialog = new LessonResultDialog();
            dialog.ViewModel = new LessonResultDialogViewModel(this);
            DialogOrganizerSingleton.Instance.ShowDialog(dialog);
        }
    }
}
