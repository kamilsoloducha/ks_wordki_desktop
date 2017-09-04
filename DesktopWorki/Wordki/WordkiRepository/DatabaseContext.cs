using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordkiRepository.Model;

namespace WordkiRepository
{
    public class DatabaseContext : DbContext
    {

        private static string DatabasePath = AppDomain.CurrentDomain.BaseDirectory + "Database\\database.sqlite";

        static DatabaseContext()
        {
            if (File.Exists(DatabasePath))
            {
                return;
            }
            SQLiteConnection.CreateFile(DatabasePath);
        }

        public DatabaseContext() : base(new SQLiteConnection($"Data Source={DatabasePath};Version=3;"), false)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}
