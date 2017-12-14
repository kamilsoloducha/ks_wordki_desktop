using Repository.Models;
using System;
using System.Linq;
using System.Collections;
using System.Windows.Input;
using Wordki.Database;
using Wordki.Helpers;
using System.Collections.Generic;

namespace Wordki.ViewModels
{
    public class WordManageViewModel : ViewModelBase
    {
        private IGroup _group;
        public IGroup Group
        {
            get { return _group; }
            set
            {
                if (_group != null && _group.Equals(value)) return;
                _group = value;
                OnPropertyChanged();
            }
        }

        public ICommand BackCommand { get; set; }
        public ICommand DeleteItemsCommand { get; set; }
        public ICommand VisibilityChangeCommnad { get; set; }

        public WordManageViewModel()
        {
            DeleteItemsCommand = new Util.BuilderCommand(DeleteItems);
            VisibilityChangeCommnad = new Util.BuilderCommand(VisibilityChange);
            BackCommand = new Util.BuilderCommand(BackAction);
        }

        public override void InitViewModel(object parameter = null)
        {
            Group = parameter as IGroup;
            if(Group == null)
            {
                Console.WriteLine("The parameter is not IGroup type. Cannot InitViewModel");
                return;
            }
        }

        public override void Back()
        {
        }

        private void VisibilityChange(object obj)
        {
            if (obj == null)
                return;
            IList list = (obj as IList);
            if (list == null)
                return;
            foreach (IWord word in list)
            {
                word.ChangeVisibility();
            }
        }

        private void BackAction()
        {
            Switcher.Back();
        }

        private void DeleteItems(object obj)
        {
            if (obj == null)
                return;
            IList list = (obj as IList);
            if (list == null)
                return;
            IList<IWord> words = list.Cast<IWord>().ToList();
            IDatabase database = DatabaseSingleton.Instance;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                database.DeleteWord(words[i]);
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
