using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;

namespace Wordki.Database2
{
    public class WordMap : ClassMapping<Word>
    {

        public WordMap()
        {
            Id(x => x.Id);
            Property(x => x.UserId);
            Property(x => x.Language1);
            Property(x => x.Language2);
            Property(x => x.Language1Comment);
            Property(x => x.Language2Comment);
            Property(x => x.Drawer);
            Property(x => x.State);

        }

    }
}
