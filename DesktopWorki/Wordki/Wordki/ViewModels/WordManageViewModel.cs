using Repository.Models;
using System;
using System.Collections;
using System.Linq;
using Wordki.Database;
using Wordki.Helpers;
using Wordki.Models;

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

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public IDatabase Database { get; set; }
        public BuilderCommand BackCommand { get; set; }
        public BuilderCommand DeleteItemsCommand { get; set; }
        public BuilderCommand VisibilityChangeCommnad { get; set; }
        public BuilderCommand ConnectItemsCommand { get; set; }

        public WordManageViewModel()
        {
            Database = DatabaseSingleton.GetDatabase();
            ActivateCommand();

        }

        public override void InitViewModel()
        {
            try
            {
                long lGroupId = (long)Util.PackageStore.Get(0);
                Group = Database.Groups.FirstOrDefault(x => x.Id == lGroupId);
                Name = "jakaś nazwa";
            }
            catch (Exception lException)
            {
                LoggerSingleton.LogError("Blad w czasie inicjalizacji WordManagerViewModel - {0}", lException.Message);
            }
        }

        public override void Back()
        {
        }

        private void ActivateCommand()
        {
            DeleteItemsCommand = new BuilderCommand(DeleteItems);
            VisibilityChangeCommnad = new BuilderCommand(VisibilityChange);
            BackCommand = new BuilderCommand(Back);
            ConnectItemsCommand = new BuilderCommand(ConnectItems);
        }

        private async void ConnectItems(object obj)
        {
            if (obj == null)
            {
                return;
            }
            IEnumerable items = obj as IEnumerable;
            if (items == null)
            {
                return;
            }
            //await Database.ConnectWords(items.OfType<IWord>().ToList());
        }

        private async void VisibilityChange(object obj)
        {
            if (obj == null)
                return;
            IList lList = (obj as IList);
            if (lList == null)
                return;
            foreach (Word lItem in lList)
            {
                lItem.Visible = !lItem.Visible;
                await Database.UpdateWordAsync(lItem);
            }
        }

        private void Back(object obj)
        {
            Switcher.Back();
        }

        private async void DeleteItems(object obj)
        {
            if (obj == null)
                return;
            IList lList = (obj as IList);
            if (lList == null)
                return;
            for (int i = lList.Count - 1; i >= 0; i--)
            {
                //await Database.DeleteWordAsync(Group, (Word)lList[i]);
            }
        }

    }
}
