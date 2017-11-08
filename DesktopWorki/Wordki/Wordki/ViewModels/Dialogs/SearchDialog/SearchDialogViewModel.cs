using Repository.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Util;
using Wordki.Database;

namespace Wordki.ViewModels.Dialogs
{
    public class SearchDialogViewModel : DialogViewModelBase
    {
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

        public override void Back()
        {
        }

        public override void InitViewModel()
        {
            CancelCommand = new BuilderCommand(Cancel);
            SearchCommand = new BuilderCommand(Search);
            Words = new ObservableCollection<IWord>();
        }

        private void Cancel(object obj)
        {
            OnClosingRequest();
        }

        private void Search(object obj)
        {
            IDatabase database = DatabaseSingleton.GetDatabase();
            IEnumerable<IWord> words = database.Groups.SelectMany(x => x.Words).Where(x => x.Language1.Contains(SearchingWord) || x.Language2.Contains(SearchingWord));
            Words.Clear();
            foreach(IWord word in words)
            {
                Words.Add(word);
            }
        }
    }
}
