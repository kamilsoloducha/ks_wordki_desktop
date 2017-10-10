using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordki.Models;

namespace Wordki.Database2
{
    public class UserMap : ClassMapping<User>
    {

        public UserMap()
        {
            Id(x => x.UserId);
            Property(x => x.Name);
            Property(x => x.Password);
            Property(x => x.IsLogin);
            Property(x => x.IsRegister);
            Property(x => x.LastLoginTime);
            Property(x => x.DownloadTime);
            Property(x => x.TranslationDirection);
            Property(x => x.AllWords);
            Property(x => x.Timeout);
            Property(x => x.ApiKey);
        }

    }
}
