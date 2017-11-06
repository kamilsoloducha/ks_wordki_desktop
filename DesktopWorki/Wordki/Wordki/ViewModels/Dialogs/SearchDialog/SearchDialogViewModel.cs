using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Util;

namespace Wordki.ViewModels.Dialogs
{
    public class SearchDialogViewModel : ViewModelBase
    {
        public event EventHandler ClosingRequest;

        private string _searchingWord;

        public string SearchingWord
        {
            get { return _searchingWord; }
            set
            {
                if(_searchingWord == value)
                {
                    return;
                }
                _searchingWord = value;
                OnPropertyChanged();
            }
        }

        public ICommand CancelCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public override void Back()
        {
        }

        public override void InitViewModel()
        {
            CancelCommand = new BuilderCommand(Cancel);
            SearchCommand = new BuilderCommand(Search);
        }

        private void Cancel(object obj)
        {
            OnClosingRequest();
        }

        private void Search(object obj)
        {

        }

        protected void OnClosingRequest()
        {
            if (ClosingRequest != null)
            {
                ClosingRequest(this, EventArgs.Empty);
            }
        }
    }
}
