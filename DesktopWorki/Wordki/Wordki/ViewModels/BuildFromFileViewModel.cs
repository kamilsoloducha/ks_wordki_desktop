using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Wordki.Database2;
using Wordki.Helpers;
using Wordki.Helpers.FileChooser;
using Wordki.Helpers.GroupCreator;
using Wordki.Models;

namespace Wordki.ViewModels
{

    public class BuildFromFileViewModel : ViewModelBase
    {

        #region Properties

        private string _fileContent;
        public string FileContent
        {
            get { return _fileContent; }
            set
            {
                if (_fileContent == value)
                    return;
                _fileContent = value;
                OnPropertyChanged();
            }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath == value)
                    return;
                _filePath = value;
                OnPropertyChanged();
            }
        }

        private string _comments;
        public string Comments
        {
            get { return _comments; }
            set
            {
                if (_comments == value)
                    return;
                _comments = value;
                OnPropertyChanged();
            }
        }

        private string _wordSeparator;
        public string WordSeparator
        {
            get { return _wordSeparator; }
            set
            {
                if (_wordSeparator == value)
                    return;
                _wordSeparator = value;
                OnPropertyChanged();
            }
        }

        private string _lineSeparator;
        public string LineSeparator
        {
            get { return _lineSeparator; }
            set
            {
                if (_lineSeparator == value)
                    return;
                _lineSeparator = value;
                OnPropertyChanged();
            }
        }

        private Group _group;

        public Group Group
        {
            get { return _group; }
            set
            {
                {
                    if (_group == value)
                        return;
                    _group = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<KeyValuePair<string, string>> _pairs;
        public ObservableCollection<KeyValuePair<string, string>> Pairs
        {
            get { return _pairs; }
            set
            {
                if (_pairs == value)
                    return;
                _pairs = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public BuilderCommand ChooseFileCommand { get; set; }
        public BuilderCommand CreateGroupCommand { get; set; }
        public BuilderCommand SaveGroupCommand { get; set; }
        public BuilderCommand BackCommand { get; set; }

        private object _pairsLock = new object();

        public BuildFromFileViewModel()
        {
            ChooseFileCommand = new BuilderCommand(ChooseFile);
            CreateGroupCommand = new BuilderCommand(CreateGroup);
            SaveGroupCommand = new BuilderCommand(SaveGroup);
            BackCommand = new BuilderCommand(Back);
            Pairs = new ObservableCollection<KeyValuePair<string, string>>();
        }

        public override void InitViewModel()
        {
            FileContent = "";
            FilePath = "";
            Comments = "";
            WordSeparator = ";";
            LineSeparator = "|";
            BindingOperations.EnableCollectionSynchronization(Pairs, _pairsLock);
        }

        public override void Back()
        {
            BindingOperations.DisableCollectionSynchronization(Pairs);
        }

        private void Back(object obj)
        {
            Back();
            Switcher.GetSwitcher().Back();
        }

        private async void SaveGroup(object obj)
        {
            IDatabase database = DatabaseSingleton.GetDatabase();
            await database.AddGroupAsync(Group);
            BackCommand.Execute(null);
        }

        private void CreateGroup(object obj)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                return;
            }
            GroupCreatorSettings settings = new GroupCreatorSettings()
            {
                ElementSeparator = ';',
                WordSeparator = '|'

            };
            IGroupCreator groupCreator = new GroupCreatorFromFile(FilePath)
            {
                Settings = settings,
                GroupNameCreator = new GroupNameCreatorFromFile()
            };

            Group = groupCreator.Create();
        }

        private void ChooseFile(object obj)
        {
            IFileChooser fileChooser = new FileChooser();
            FilePath = fileChooser.Choose();
        }

    }
}
