using FluentNHibernate.Mapping;
using Wordki.Models;

namespace Wordki.Database
{
    public class WordMap : ClassMap<Word>
    {

        public WordMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Language1);
            Map(x => x.Language2);
            Map(x => x.Language1Comment);
            Map(x => x.Language2Comment);
            Map(x => x.Drawer);
            Map(x => x.Visible);
            Map(x => x.Selected);
            Map(x => x.Comment);
            Map(x => x.LastRepeating);
            Map(x => x.RepeatingNumber);
            Map(x => x.State);
            References(x => x.Group).Not.Nullable().Class(typeof(Group));
        }
    }
}
