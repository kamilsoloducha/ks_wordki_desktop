using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wordki.Database2.Organizer
{
    public class DatabaseOrganizer : IDatabaseOrganizer
    {

        private string _mainPath;

        public DatabaseOrganizer(string mainPath)
        {
            _mainPath = mainPath;
        }

        public IEnumerable<string> GetDatabases()
        {
            string[] files = Directory.GetFiles(_mainPath);
            foreach(string file in files)
            {
                string fileName = Path.GetFileName(file);
                if (Path.GetExtension(fileName).Equals("db"))
                {
                    yield return fileName.Replace(".db", "");
                }
            }
        }
    }
}
