using System;
using Repository.Models;
using System.IO;
using System.Collections.Generic;

namespace Wordki.Database2
{
    public class DatabaseOrganizer : IDatabaseOrganizer
    {

        public IDatabase Database { get; set; }
        private string _mainPath;

        public DatabaseOrganizer(string mainPath)
        {
            Database = DatabaseSingleton.GetDatabase();
            _mainPath = mainPath;
        }

        public bool AddDatabase(IUser user)
        {
            try
            {
                if (CheckDatabase(user))
                {
                    return true;
                }
                NHibernateHelper.ResetSession();
                NHibernateHelper.DatabaseName = user.Name;
                return Database.AddUserAsync(user).Result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase(IUser user)
        {
            try
            {
                if (!CheckDatabase(user))
                {
                    return false;
                }
                NHibernateHelper.ResetSession();
                File.Delete(NHibernateHelper.DatabasePath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CheckDatabase(IUser user)
        {
            try
            {
                NHibernateHelper.DatabaseName = user.Name;
                NHibernateHelper.ResetSession();
                IUser userFromDatabase = Database.GetUserAsync(user.Name, user.Password).Result;
                if (userFromDatabase != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<string> GetDatabases()
        {
            string[] files = Directory.GetFiles(_mainPath);
            foreach (string file in files)
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
