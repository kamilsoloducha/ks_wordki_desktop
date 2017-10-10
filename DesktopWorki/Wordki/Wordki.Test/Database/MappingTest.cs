using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Database2;

namespace Wordki.Test.Database
{
    [TestFixture]
    public class NHibernateMappingTest
    {
        [Test]
        public void CanGenerateSchema()
        {
            var schemaUpdate = new SchemaUpdate(NHibernateHelper.Configuration);
            schemaUpdate.Execute(Console.WriteLine, true);
        }
    }
}
