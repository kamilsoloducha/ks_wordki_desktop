using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
