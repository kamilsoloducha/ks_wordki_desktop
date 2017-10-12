using FluentNHibernate.Mapping;
using Wordki.Models;

namespace Wordki.Database2
{
    public class UserMap : ClassMap<User>
    {

        public UserMap()
        {
            Id(x => x.UserId);
            Map(x => x.Name);
            Map(x => x.Password);
            Map(x => x.IsLogin);
            Map(x => x.IsRegister);
            Map(x => x.LastLoginTime);
            Map(x => x.DownloadTime);
            Map(x => x.TranslationDirection);
            Map(x => x.AllWords);
            Map(x => x.Timeout);
            Map(x => x.ApiKey);
        }

    }
}
