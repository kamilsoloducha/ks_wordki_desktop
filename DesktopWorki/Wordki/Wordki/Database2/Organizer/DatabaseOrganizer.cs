using System.Collections.Generic;
using System.IO;

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
                if (Path.GetExtension(file).Equals(".db"))
                {
                    yield return fileName.Replace(".db", "");
                }
            }
        }
    }
}
