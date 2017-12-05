using Repository.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Wordki.Database;
using Wordki.Helpers;
using Wordki.Helpers.WordComparer;
using Wordki.Helpers.WordConnector;
using Wordki.Helpers.WordFinder;
using Wordki.InteractionProvider;
using Wordki.Models;
using Wordki.Views.Dialogs;

namespace Wordki.ViewModels
{
    public class SameWordsViewModel : ViewModelBase
    {

        private IDatabase Database { get; set; }
        public ObservableCollection<Word> DataGridCollection { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ConnectWordsCommand { get; set; }
        public ICommand EditWordCommand { get; set; }
        public ICommand BothLanguagesCommand { get; set; }

        private object _lockObject = new object();

        public SameWordsViewModel()
        {
            ActivateCommand();
            DataGridCollection = new ObservableCollection<Word>();
        }

        private void ActivateCommand()
        {
            BackCommand = new Util.BuilderCommand(BackAction);
            ConnectWordsCommand = new Util.BuilderCommand(ConnectWord);
            EditWordCommand = new Util.BuilderCommand(EditWord);
            BothLanguagesCommand = new Util.BuilderCommand(BothLanguages);
        }

        private void EditWord(object obj)
        {
            if (obj == null)
            {
                return;
            }
            IWord word = obj as IWord;
            if (word == null)
            {
                return;
            }
            IInteractionProvider provider = new CorrectWordProvider()
            {
                Word = word,
            };
            provider.Interact();
        }

        private void BothLanguages(object obj)
        {
            CreateDataGridCollection();
        }

        public override void InitViewModel()
        {
            Database = DatabaseSingleton.Instance;
        }

        public override void Back()
        {

        }


        private void ConnectWord(object obj)
        {
            if (obj == null)
                return;
            IList lList = (obj as IList);
            if (lList == null)
                return;
            IWordConnector connector = new WordConnector();
            connector.Connect(lList.Cast<Word>());
            Database.SaveDatabaseAsync();
        }

        private void BackAction(object obj)
        {
            Switcher.Back();
        }

        private async void CreateDataGridCollection()
        {
            BindingOperations.EnableCollectionSynchronization(Database.Groups, _lockObject);
            await Task.Run(() =>
            {
                DataGridCollection.Clear();
                IEnumerable<Word> lSameWords = FindSameWords();
                foreach (Word lWord in lSameWords)
                {
                    IGroup lGroup = Database.Groups.FirstOrDefault(x => x.Id == lWord.Group.Id);
                    if (lGroup == null)
                        continue;
                    DataGridCollection.Add(lWord);
                }
            });
            BindingOperations.DisableCollectionSynchronization(Database.Groups);
        }

        private IEnumerable<Word> FindSameWords()
        {
            try
            {
                IEnumerable<IWord> words = Database.Groups.SelectMany(x => x.Words);
                IWordComparer wordComparer = new WordComparer();
                IWordFinder wordFinder = new WordFinder(words, wordComparer);
                return wordFinder.FindWords();
            }
            catch (Exception lException)
            {
                LoggerSingleton.LogError("{0} - {1}", "BuilderViewModel.FindSame", lException.Message);
                return Enumerable.Empty<Word>();
            }
        }

        public override void Loaded()
        {
            
        }

        public override void Unloaded()
        {
            
        }
    }
}
