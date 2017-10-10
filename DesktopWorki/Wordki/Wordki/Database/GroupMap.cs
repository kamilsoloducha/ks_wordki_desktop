using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Wordki.Models;

namespace Wordki.Database2
{
    public class GroupMap : ClassMapping<Group>
    {

        public GroupMap()
        {
            Table("grupa");
            Id(x => x.Id);
            Property(x => x.UserId);
            Property(x => x.Name);
            Property(x => x.Language1);
            Property(x => x.Language2);
            Property(x => x.State);
            ManyToOne(x => x.Words, m =>
            {
                m.Column("column_name");
                m.Class(typeof(Word));
                m.Cascade(Cascade.All | Cascade.None | Cascade.Persist | Cascade.Remove);
                m.Fetch(FetchKind.Join); // or FetchKind.Select
                m.Update(true);
                m.Insert(true);
                m.Access(Accessor.Field);
                m.Unique(true);
                m.OptimisticLock(true);

                m.Lazy(LazyRelation.Proxy);

                //m.PropertyRef ???
                //m.NotFound ???

                m.Index("column_idx");
                m.NotNullable(true);
                m.UniqueKey("column_uniq");
            });
        }

    }
}
