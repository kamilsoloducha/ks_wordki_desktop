using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordkiModel.Extensions
{
    public static class GroupsExtensions
    {

        public static IGroup AddWord(this IGroup group, IWord word)
        {
            group.Words.Add(word);
            word.Group = group;
            return group;
        }

        public static IGroup AddResult(this IGroup group, IResult result)
        {
            group.Results.Add(result);
            result.Group = group;
            return group;
        }

    }
}
