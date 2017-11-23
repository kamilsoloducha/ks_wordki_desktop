using FluentNHibernate.Mapping;
using Repository.Models;
using Wordki.Models;

namespace Wordki.Database
{
    public class ResultMap : ClassMap<Result>
    {
        public ResultMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Accepted);
            Map(x => x.Correct);
            Map(x => x.Wrong);
            Map(x => x.DateTime);
            Map(x => x.Invisibilities);
            Map(x => x.LessonType);
            Map(x => x.TimeCount);
            Map(x => x.TranslationDirection);
            Map(x => x.State);
            References(x => x.Group).Not.Nullable().Class(typeof(Group));
        }

    }
}
