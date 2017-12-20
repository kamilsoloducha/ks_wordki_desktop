namespace Wordki.ViewModels.Dialogs
{
    public class DialogWindowViewModel : DialogViewModelBase
    {

        private object page;
        public object Page
        {
            get { return page; }
            set
            {
                if (page == value)
                {
                    return;
                }
                page = value;
                OnPropertyChanged();
            }
        }

        public DialogWindowViewModel() : base()
        {
            CloseCommand = new Util.BuilderCommand(Close);
        }

    }
}
