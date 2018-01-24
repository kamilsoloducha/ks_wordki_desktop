using WordkiModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Util;
using Wordki.Database;
using Wordki.Commands;

namespace Wordki.ViewModels.Dialogs
{
    public class SearchDialogViewModel : DialogViewModelBase
    {

        private object _wordsLock = new object();
        private string _searchingWord;

        public string SearchingWord
        {
            get { return _searchingWord; }
            set
            {
                if (_searchingWord == value)
                {
                    return;
                }
                _searchingWord = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<IWord> _words;

        public ObservableCollection<IWord> Words
        {
            get { return _words; }
            set { _words = value; }
        }

        public ICommand CancelCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand MouseDoubleClickCommand { get; set; }

        public override void InitViewModel(object parameter = null)
        {
            CancelCommand = new BuilderCommand(Cancel);
            SearchCommand = new BuilderCommand(Search);
            MouseDoubleClickCommand = new BuilderCommand((obj) => ActionsSingleton<CheckUncheckAction>.Instance.Action(obj as IWord));
            Words = new ObservableCollection<IWord>();
            BindingOperations.EnableCollectionSynchronization(Words, _wordsLock);
        }

        private void Cancel(object obj)
        {
            OnClosingRequest();
        }

        private void Search(object obj)
        {
            if (SearchingWord == null)
            {
                Helpers.LoggerSingleton.LogError("Serching word is null");
                return;
            }
            Task.Run(() =>
            {
                string searchingWordLowerCase = SearchingWord.ToLower();
                IDatabase database = DatabaseSingleton.Instance;
                IEnumerable<IWord> words = database.Groups.SelectMany(x => x.Words).
                Where(x => x.Language1.ToLower().Contains(searchingWordLowerCase) || x.Language2.ToLower().Contains(searchingWordLowerCase));
                Words.Clear();
                foreach (IWord word in words)
                {
                    Words.Add(word);
                }
            });
        }
    }
}
