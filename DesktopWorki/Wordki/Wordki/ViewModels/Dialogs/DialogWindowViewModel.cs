using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
