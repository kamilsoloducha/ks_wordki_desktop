using System;
using System.Collections.Generic;
using System.Data.Common;
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
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>(true));
            //Console.Out.WriteLine(DatabasePath);
            //if (File.Exists(DatabasePath))
            //{
            //    return;
            //}
            //SQLiteConnection.CreateFile(DatabasePath);
        }

        public DatabaseContext() : base(new SQLiteConnection($"data source={DatabasePath}; Version=3;"), false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Result> Results { get; set; }

    }
}
