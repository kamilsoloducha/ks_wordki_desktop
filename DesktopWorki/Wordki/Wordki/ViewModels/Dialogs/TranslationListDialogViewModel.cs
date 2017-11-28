using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Wordki.ViewModels.Dialogs
{
    public class TranslationListDialogViewModel : DialogViewModelBase
    {

        public ICommand OkCommand { get; set; }
        public List<TranslationItem> Items { get; set; }
        public bool Canceled { get; set; }


        public TranslationListDialogViewModel(IEnumerable<string> items)
        {
            Items = new List<TranslationItem>();
            foreach (string item in items)
            {
                Items.Add(new TranslationItem()
                {
                    Translation = item,
                    Approved = false,
                });
            }
        }

        public override void InitViewModel()
        {
            base.InitViewModel();
            CloseCommand = new Util.BuilderCommand(Close);
            OkCommand = new Util.BuilderCommand(Ok);
        }

        private void Ok(object obj)
        {
            Canceled = false;
            OnClosingRequest();
        }

        protected override void Close()
        {
            base.Close();
            Canceled = true;
        }

        public class TranslationItem : INotifyPropertyChanged
        {
            private bool _approved;

            public bool Approved
            {
                get { return _approved; }
                set
                {
                    if (_approved == value)
                    {
                        return;
                    }
                    _approved = value;
                    OnPropertyChanged();
                }
            }

            private string _translation;

            public string Translation
            {
                get { return _translation; }
                set
                {
                    if (_translation == value)
                    {
                        return;
                    }
                    _translation = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void OnPropertyChanged([CallerMemberName] string name = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
                }
            }
        }
    }

}
