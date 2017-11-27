using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;
using System.Windows.Input;
using Wordki.Models;
using Wordki.Database;

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
        public ICommand CorrectCommad { get; }
        public ICommand RemoveCommand { get; }

        private IWord originalWord;
        private IDatabase database;

        public CorrectWordDialogViewModel(IWord word)
        {
            Word = word.Clone() as IWord;
            originalWord = word;
            CorrectCommad = new Util.BuilderCommand(Correct);
            RemoveCommand = new Util.BuilderCommand(Remove);
            database = DatabaseSingleton.Instance;
        }

        public void Correct(object obj)
        {
            IWord word = obj as IWord;
            if (word == null)
            {
                return;
            }
            originalWord.Language1 = word.Language1;
            originalWord.Language2 = word.Language2;
            originalWord.Language1Comment = word.Language1Comment;
            originalWord.Language2Comment = word.Language2Comment;
            originalWord.Checked = word.Checked;
            originalWord.Visible = word.Visible;
            database.UpdateWordAsync(originalWord);
        }

        public void Remove(object obj)
        {
            IWord word = obj as IWord;
            if (word == null)
            {
                return;
            }
            database.DeleteWordAsync(word);
        }



    }
}
