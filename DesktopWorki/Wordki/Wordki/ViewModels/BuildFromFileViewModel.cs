using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;
using Wordki.Helpers;
using Wordki.Models;

namespace Wordki.ViewModels
{

    public class BuildFromFileViewModel : INotifyPropertyChanged, IViewModel
    {

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string pPropertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(pPropertyName));
            }
        }
        #endregion

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

        public void InitViewModel()
        {
            FileContent = "";
            FilePath = "";
            Comments = "";
            WordSeparator = ";";
            LineSeparator = "|";
            BindingOperations.EnableCollectionSynchronization(Pairs, _pairsLock);
        }

        public void Back()
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
            Database database = Database.GetDatabase();
            Group newGroup = new Group
            {
                Name = Path.GetFileName(FilePath),
            };
            if (!(await database.AddGroupAsync(newGroup)))
            {
                Logger.LogError("Błąd dodania grupy");
                return;
            }
            foreach (var pair in Pairs)
            {
                Word newWord = new Word
                {
                    GroupId = newGroup.Id,
                    Language1 = pair.Key,
                    Language2 = pair.Value,
                };
                newGroup.WordsList.Add(newWord);
                await database.AddWordAsync(newGroup, newWord);
            }
            BackCommand.Execute(null);
        }

        private void CreateGroup(object obj)
        {
            Task.Run(() =>
            {
                char? lineSeparator = GetSeparator(LineSeparator);
                if (!lineSeparator.HasValue)
                {
                    return;
                }
                char? wordSeparator = GetSeparator(WordSeparator);
                if (!wordSeparator.HasValue)
                {
                    return;
                }
                if (!FileContent.Contains(LineSeparator))
                {
                    return;
                }
                string[] lines = FileContent.Split(lineSeparator.Value);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    if (!line.Contains(WordSeparator))
                    {
                        continue;
                    }
                    string[] word = line.Split(wordSeparator.Value);
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>(word[0].Trim(' ', '\r', '\n'), word[1].Trim(' ', '\r', '\n'));
                    Pairs.Add(pair);
                }
            });
        }

        private void ChooseFile(object obj)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "Wybierz plik";
            bool? isOpen = fileDialog.ShowDialog();
            if (isOpen.HasValue && isOpen.Value)
            {
                FilePath = fileDialog.FileName;
                FileContent = File.ReadAllText(FilePath);
            }
        }


        private char? GetSeparator(string pSeparator)
        {
            char separator;
            if (!Char.TryParse(pSeparator, out separator))
            {
                return null;
            }
            return separator;
        }



    }
}
