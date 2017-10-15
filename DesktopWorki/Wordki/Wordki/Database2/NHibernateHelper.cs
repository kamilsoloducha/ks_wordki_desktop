using System.IO;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Wordki.Database2
{
    public static class NHibernateHelper
    {
        public static string DirectoryPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Wordki");
        public static string DatabaseName = "database";
        public static string DatabasePath { get { return Path.Combine(DirectoryPath, $"{DatabaseName}.db"); } }

        private static ISessionFactory _sessionFactory;

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void ResetSession()
        {
            if (_sessionFactory != null)
            {
                _sessionFactory.Close();
            }
            _sessionFactory = null;
        }

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    CheckDirectory();
                    _sessionFactory = CreateSessionFactory();
                }
                return _sessionFactory;
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return FluentNHibernate.Cfg.Fluently.Configure()
              .Database(
                SQLiteConfiguration.Standard
                  .ConnectionString($"Data Source={DatabasePath};Version=3;New=True").ShowSql
              )
              .Mappings(m =>
              {
                  m.FluentMappings.AddFromAssemblyOf<UserMap>();
                  m.FluentMappings.AddFromAssemblyOf<GroupMap>();
                  m.FluentMappings.AddFromAssemblyOf<WordMap>();
                  m.FluentMappings.AddFromAssemblyOf<ResultMap>();
              })
              .ExposeConfiguration(cfg => new NHibernate.Tool.hbm2ddl.SchemaUpdate(cfg).Execute(true, true))
              .BuildSessionFactory();
        }

        public static void ClearDatabase()
        {
            ExecuteSQL("DELETE FROM 'Result'");
            ExecuteSQL("DELETE FROM 'Word'");
            ExecuteSQL("DELETE FROM 'Group'");
            ExecuteSQL("DELETE FROM 'User'");
        }

        public static void RefreshDatabase()
        {
            ExecuteSQL("DELETE FROM 'Result' WHERE State < 0");
            ExecuteSQL("DELETE FROM 'Word' WHERE State < 0");
            ExecuteSQL("DELETE FROM 'Group' WHERE State < 0");

            ExecuteSQL("UPDATE 'Result' SET State = 0");
            ExecuteSQL("UPDATE 'Word' SET State = 0");
            ExecuteSQL("UPDATE 'Group' SET State = 0");
        }

        private static void ExecuteSQL(string query)
        {
            using (ISession session = OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.CreateSQLQuery(query).ExecuteUpdate();
                transaction.Commit();
            }
        }

        private static void CheckDirectory()
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }


    }
}