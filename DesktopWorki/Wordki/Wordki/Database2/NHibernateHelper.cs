using System.IO;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Wordki.Database2
{
    public static class NHibernateHelper
    {
        public static string DirectoryPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Wordki");
        public static string DatabasePath = Path.Combine(DirectoryPath, "database2.db");
        private static ISessionFactory _sessionFactory;

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void ResetSession()
        {
            if(_sessionFactory != null)
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
                  //m.FluentMappings.AddFromAssemblyOf<GroupMap>();
                  //m.FluentMappings.AddFromAssemblyOf<WordMap>();
                  //m.FluentMappings.AddFromAssemblyOf<ResultMap>();

              })
              .ExposeConfiguration(cfg => new NHibernate.Tool.hbm2ddl.SchemaExport(cfg).Execute(true, true, false))
              .BuildSessionFactory();
        }

        public static void ClearDatabase()
        {
            using (ISession session = OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.CreateSQLQuery("DELETE FROM 'Result'").ExecuteUpdate();
                transaction.Commit();
            }
            using (ISession session = OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.CreateSQLQuery("DELETE FROM 'Word'").ExecuteUpdate();
                transaction.Commit();
            }
            using (ISession session = OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.CreateSQLQuery("DELETE FROM 'Group'").ExecuteUpdate();
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