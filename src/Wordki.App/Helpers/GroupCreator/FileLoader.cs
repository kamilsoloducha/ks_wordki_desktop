using System.IO;

namespace Wordki.Helpers.GroupCreator
{
    public class FileLoader : IFileLoader
    {

        public string LoadFile(string path)
        {
            string content;
            using(StreamReader reader = new StreamReader(path))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }

    }
}
