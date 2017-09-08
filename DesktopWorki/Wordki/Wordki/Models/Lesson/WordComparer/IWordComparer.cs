using System.Collections.Generic;

namespace Wordki.Models.Lesson.WordComparer
{
    public interface IWordComparer
    {

        ICollection<INotCheck> NotCheckers { get; }

        bool Compare(string word1, string word2);

    }
}
