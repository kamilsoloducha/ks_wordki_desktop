using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordkiRepository
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DatabaseContext : DbContext
    {

        public DatabaseContext() : base (String.Format("Data Source={0};", "test.db"))
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }

    }


    public class Test
    {
        public void Testing()
        {
            using(DatabaseContext context = new DatabaseContext())
            {
                context.Users.Add(new User()
                {
                    Id = 1,
                    Name = "test"
                });
                context.SaveChangesAsync();
            }
        }
    }
}
