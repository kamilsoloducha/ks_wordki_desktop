using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Wordki.Database2
{
    public static class NHibernateHelper
    {
        public static string DirectoryPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Wordki");
        public static string DatabasePath = Path.Combine(DirectoryPath, "database.db");
        private static ISessionFactory _sessionFactory;

        public static ISession OpenSession()
        {
            //Open and return the nhibernate session
            return SessionFactory.OpenSession();
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
                  .ConnectionString($"Data Source={DatabasePath};Version=3;New=True")
              )
              .Mappings(m =>
              {
                  m.FluentMappings.AddFromAssemblyOf<UserMap>();
                  m.FluentMappings.AddFromAssemblyOf<GroupMap>();
                  m.FluentMappings.AddFromAssemblyOf<WordMap>();

              })
              .ExposeConfiguration(cfg => new NHibernate.Tool.hbm2ddl.SchemaExport(cfg).Execute(true, true, false))
              .BuildSessionFactory();
        }

        public static void ClearDatabase()
        {
            //using (ISession session = OpenSession())
            //using (ITransaction transaction = session.BeginTransaction())
            //{
            //    session.CreateSQLQuery("DELETE FROM Result").ExecuteUpdate();
            //    transaction.Commit();
            //}
            using (ISession session = OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                //session.CreateSQLQuery("DELETE FROM Result").ExecuteUpdate();
                //transaction.Commit();
                session.CreateSQLQuery("DELETE FROM 'Word'").ExecuteUpdate();
                transaction.Commit();
            }
            using (ISession session = OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                //session.CreateSQLQuery("DELETE FROM Result").ExecuteUpdate();
                //transaction.Commit();
                session.CreateSQLQuery("DELETE FROM 'Group'").ExecuteUpdate();
                transaction.Commit();
            }
        }

        //public static Configuration Configuration
        //{
        //    get
        //    {
        //        if (_configuration == null)
        //        {
        //            //Create the nhibernate configuration
        //            _configuration = CreateConfiguration();
        //        }
        //        return _configuration;
        //    }
        //}

        //public static HbmMapping Mapping
        //{
        //    get
        //    {
        //        if (_mapping == null)
        //        {
        //            //Create the mapping
        //            _mapping = CreateMapping();
        //        }
        //        return _mapping;
        //    }
        //}

        //private static Configuration CreateConfiguration()
        //{
        //    var configuration = new Configuration()
        //        .AddProperties(new Dictionary<string, string> {
        //            {Environment.ConnectionDriver, typeof (NHibernate.Driver.SQLite20Driver).FullName},
        //            {Environment.Dialect, typeof (NHibernate.Dialect.SQLiteDialect).FullName},
        //            {Environment.ConnectionProvider, typeof (NHibernate.Connection.DriverConnectionProvider).FullName},
        //            {Environment.ConnectionString, $"Data Source={DatabasePath};Version=3;New=True"},
        //        })
        //        .AddAssembly(System.Reflection.Assembly.GetExecutingAssembly());

        //    //Loads nhibernate mappings 
        //    configuration.AddDeserializedMapping(Mapping, null);

        //    return configuration;
        //}

        //private static HbmMapping CreateMapping()
        //{
        //    var mapper = new ModelMapper();
        //    //Add the person mapping to the model mapper
        //    mapper.AddMappings(new List<System.Type> { typeof(WordMap) });
        //    mapper.AddMappings(new List<System.Type> { typeof(GroupMap), });
        //    //mapper.AddMappings(new List<System.Type> { typeof(WordMap) });
        //    //Create and return a HbmMapping of the model mapping in code
        //    return mapper.CompileMappingForAllExplicitlyAddedEntities();
        //}

        private static void CheckDirectory()
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }


    }
}