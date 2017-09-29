using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;

namespace Wordki.Helpers.GroupCreator
{
    public class GroupCreatorFromFile : IGroupCreator
    {
        private FileLoader _fileLoader;
        private string _filePath;
        public IGroupNameCreator GroupNameCreator { get; set; }
        public GroupCreatorSettings Settings { get; set; }


        public GroupCreatorFromFile(string path)
        {
            _filePath = path;
            _fileLoader = new FileLoader();
        }

        public Group Create()
        {
            Group group = new Group();
            string fileContent = string.Empty;
            try
            {
                fileContent = _fileLoader.LoadFile(_filePath);
            }
            catch (Exception)
            {
                return group;
            }

            if (string.IsNullOrEmpty(fileContent))
            {
                return group;
            }

            string[] lines = fileContent.Split(Settings.WordSeparator);
            foreach(string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                string[] elements = line.Split(Settings.ElementSeparator);
                Word word = new Word();
                if(elements.Length > 1)
                {
                    SetLanguages(word, elements);
                }
                if(elements.Length > 3)
                {
                    SetComments(word, elements);
                }
                group.WordsList.Add(word);

            }
            return group;
        }

        private void SetLanguages(Word word, string[] elements)
        {
            word.Language1 = elements[0];
            word.Language2 = elements[1];
        }

        private void SetComments(Word word, string[] elements)
        {
            word.Language1Comment = elements[2];
            word.Language2Comment = elements[3];
        }

    }
}
