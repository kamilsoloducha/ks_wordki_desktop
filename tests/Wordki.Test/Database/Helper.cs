using System.IO;
using Wordki.Database;

namespace Wordki.Test.Database
{
    public static class Helper
    {

        public static void RemoveDatabaseFile()
        {
            if (File.Exists(NHibernateHelper.DatabasePath))
                File.Delete(NHibernateHelper.DatabasePath);
        }

    }
}
