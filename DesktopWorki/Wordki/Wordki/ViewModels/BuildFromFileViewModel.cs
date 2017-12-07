using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using Wordki.Database;
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

        public ICommand ChooseFileCommand { get; set; }
        public ICommand CreateGroupCommand { get; set; }
        public ICommand SaveGroupCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private object _pairsLock = new object();

        public BuildFromFileViewModel()
        {
            ChooseFileCommand = new Util.BuilderCommand(ChooseFile);
            CreateGroupCommand = new Util.BuilderCommand(CreateGroup);
            SaveGroupCommand = new Util.BuilderCommand(SaveGroup);
            BackCommand = new Util.BuilderCommand(BackAction);
            Pairs = new ObservableCollection<KeyValuePair<string, string>>();
        }

        public override void InitViewModel(object parameter = null)
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

        private void BackAction()
        {
            Back();
            Switcher.Back();
        }

        private async void SaveGroup(object obj)
        {
            IDatabase database = DatabaseSingleton.Instance;
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

        public override void Loaded()
        {
            
        }

        public override void Unloaded()
        {
            
        }
    }
}
