using System;
using WordkiModel;
using System.Windows.Input;
using Wordki.Database;
using Wordki.InteractionProvider;

namespace Wordki.ViewModels.Dialogs
{
    public class CorrectWordDialogViewModel : DialogViewModelBase
    {

        private IWord word;
        public IWord Word
        {
            get { return word; }
            set
            {
                if (word == value)
                {
                    return;
                }
                word = value;
                OnPropertyChanged();
            }
        }

        public Action CorrectWord { get; set; }
        public Action RemoveWord { get; set; }
        public ICommand CorrectCommand { get; }
        public ICommand RemoveCommand { get; }

        private IWord originalWord;
        private IDatabase database;

        public CorrectWordDialogViewModel(IWord word) : base()
        {
            Word = word.Clone() as IWord;
            originalWord = word;
            CorrectCommand = new Util.BuilderCommand(Correct);
            RemoveCommand = new Util.BuilderCommand(Remove);
            database = DatabaseSingleton.Instance;
        }

        public CorrectWordDialogViewModel(ICorrectWordProvider provdier) : this(provdier.Word)
        {
            CorrectWord = provdier.OnCorrect;
            RemoveWord = provdier.OnRemove;
            CloseAction = provdier.OnClose;
        }

        public void Correct()
        {
            originalWord.Language1 = Word.Language1;
            originalWord.Language2 = Word.Language2;
            originalWord.Language1Comment = Word.Language1Comment;
            originalWord.Language2Comment = Word.Language2Comment;
            originalWord.IsSelected = Word.IsSelected;
            originalWord.IsVisible = Word.IsVisible;
            database.UpdateWordAsync(originalWord);
            CorrectWord?.Invoke();
            Close();
        }

        public void Remove(object obj)
        {
            IWord word = obj as IWord;
            if (word == null)
            {
                return;
            }
            database.DeleteWordAsync(word);
            RemoveWord?.Invoke();
            Close();
        }



    }
}
