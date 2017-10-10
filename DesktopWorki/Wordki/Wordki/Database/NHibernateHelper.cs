using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate;
using System.IO;
using System.Data.SQLite;

namespace Wordki.Database2
{
    public static class NHibernateHelper
    {
        public static string DirectoryPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Wordki");
        public static string DatabasePath = Path.Combine(DirectoryPath, "database.db");
        private static ISessionFactory _sessionFactory;
        private static Configuration _configuration;
        private static HbmMapping _mapping;

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
                    _sessionFactory = Configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    //Create the nhibernate configuration
                    _configuration = CreateConfiguration();
                }
                return _configuration;
            }
        }

        public static HbmMapping Mapping
        {
            get
            {
                if (_mapping == null)
                {
                    //Create the mapping
                    _mapping = CreateMapping();
                }
                return _mapping;
            }
        }

        private static Configuration CreateConfiguration()
        {
            var configuration = new Configuration()
                .AddProperties(new Dictionary<string, string> {
                    {Environment.ConnectionDriver, typeof (NHibernate.Driver.SQLite20Driver).FullName},
                    {Environment.Dialect, typeof (NHibernate.Dialect.SQLiteDialect).FullName},
                    {Environment.ConnectionProvider, typeof (NHibernate.Connection.DriverConnectionProvider).FullName},
                    {Environment.ConnectionString, $"Data Source={DatabasePath};Version=3;New=True"},
                })
                .AddAssembly(System.Reflection.Assembly.GetExecutingAssembly());

            //Loads nhibernate mappings 
            configuration.AddDeserializedMapping(Mapping, null);

            return configuration;
        }

        private static HbmMapping CreateMapping()
        {
            var mapper = new ModelMapper();
            //Add the person mapping to the model mapper
            mapper.AddMappings(new List<System.Type> { typeof(WordMap) });
            mapper.AddMappings(new List<System.Type> { typeof(GroupMap), });
            //mapper.AddMappings(new List<System.Type> { typeof(WordMap) });
            //Create and return a HbmMapping of the model mapping in code
            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }

        private static void CheckDirectory()
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            if (!File.Exists(DatabasePath))
            {
                SQLiteConnection.CreateFile(DatabasePath);
            }
        }


    }
}