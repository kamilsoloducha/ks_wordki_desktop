using System;
using System.Windows.Input;

namespace Wordki.ViewModels.Dialogs
{
    public class TextBoxDialogViewModel : DialogViewModelBase
    {

        public ICommand PositiveCommand { get; private set; }
        public ICommand NegativeCommand { get; private set; }
        public Action PositiveAction { get; set; }
        public Action NegativeAction { get; set; }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (text == value)
                {
                    return;
                }
                text = value;
                OnPropertyChanged();
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                if (message == value)
                {
                    return;
                }
                message = value;
                OnPropertyChanged();
            }
        }



        public TextBoxDialogViewModel()
        {
            PositiveCommand = new Util.BuilderCommand(Positive);
            NegativeCommand = new Util.BuilderCommand(Negative);
        }

        private void Negative(object obj)
        {
            if (NegativeAction != null)
            {
                NegativeAction.Invoke();
            }
            Close();
        }

        private void Positive(object obj)
        {
            if (PositiveAction != null)
            {
                PositiveAction.Invoke();
            }
            Close();
        }
    }
}
