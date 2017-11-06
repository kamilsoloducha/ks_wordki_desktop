using FluentNHibernate.Mapping;
using Repository.Models;
using Wordki.Models;

namespace Wordki.Database
{
    public class GroupMap : ClassMap<Group>
    {

        public GroupMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name);
            Map(x => x.Language1);
            Map(x => x.Language2);
            Map(x => x.State);
            HasMany<Word>(x => x.Words)
                .Inverse()
                .Cascade.All();
            HasMany<Result>(x => x.Results)
                .Inverse()
                .Cascade.All();
        }
    }
}
