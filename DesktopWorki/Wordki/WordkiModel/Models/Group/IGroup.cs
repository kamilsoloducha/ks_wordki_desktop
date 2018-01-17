using System;
using System.Collections.Generic;

namespace WordkiModel
{
    public interface IGroup
    {
        [PropertyIndex(0)]
        long Id { get; set; }

        [PropertyIndex(2)]
        string Name { get; set; }

        [PropertyIndex(3)]
        LanguageType Language1 { get; set; }

        [PropertyIndex(4)]
        LanguageType Language2 { get; set; }

        [PropertyIndex(5)]
        int State { get; set; }

        [PropertyIndex(6)]
        DateTime CreationDate { get; set; }

        IList<IWord> Words { get; set; }
        IList<IResult> Results { get; set; }

        void AddWord(IWord word);
        void AddResult(IResult result);
    }
}