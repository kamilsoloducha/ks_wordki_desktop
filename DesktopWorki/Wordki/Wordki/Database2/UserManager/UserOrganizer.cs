using System;
using Repository.Models;
using System.IO;

namespace Wordki.Database2
{
    public class UserOrganizer : IUserOrganizer
    {

        public IDatabase Database { get; set; }

        public UserOrganizer()
        {
            Database = DatabaseSingleton.GetDatabase();
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
    }
}
