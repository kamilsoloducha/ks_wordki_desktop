using System;
using Repository.Models;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wordki.Database
{
    public class DatabaseOrganizer : IDatabaseOrganizer
    {

        public IDatabase Database { get; set; }
        private string _mainPath;

        public DatabaseOrganizer(string mainPath)
        {
            Database = DatabaseSingleton.Instance;
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
                Task<bool> task = Database.AddUserAsync(user);
                task.Wait();
                return task.Result;
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
                Task<IUser> task = Database.GetUserAsync(user.Name, user.Password);
                task.Wait();
                return task.Result != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<string> GetDatabases()
        {
            if (!Directory.Exists(_mainPath))
            {
                Directory.CreateDirectory(_mainPath);
                yield break;
            }
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

        public async Task<bool> CheckDatabaseAsync(IUser user)
        {
            try
            {
                NHibernateHelper.DatabaseName = user.Name;
                NHibernateHelper.ResetSession();
                return await Database.GetUserAsync(user.Name, user.Password) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddDatabaseAsync(IUser user)
        {
            try
            {
                if (await CheckDatabaseAsync(user))
                {
                    return true;
                }
                NHibernateHelper.ResetSession();
                NHibernateHelper.DatabaseName = user.Name;
                return await Database.AddUserAsync(user);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveDatabaseAsync(IUser user)
        {
            try
            {
                if (!await CheckDatabaseAsync(user))
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
    }
}
