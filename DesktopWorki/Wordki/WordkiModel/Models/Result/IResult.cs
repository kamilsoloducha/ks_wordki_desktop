using System;
using Newtonsoft.Json;
using WordkiModel.Enums;

namespace WordkiModel
{
    public interface IResult
    {
        [PropertyIndex(0)]
        long Id { get; set; }

        [PropertyIndex(2)]
        IGroup Group { get; set; }

        [PropertyIndex(3)]
        short Correct { get; set; }

        [PropertyIndex(4)]
        short Accepted { get; set; }

        [PropertyIndex(5)]
        short Wrong { get; set; }

        [PropertyIndex(6)]
        short Invisible { get; set; }

        [PropertyIndex(7)]
        short TimeCount { get; set; }

        [PropertyIndex(8)]
        TranslationDirection TranslationDirection { get; set; }

        [PropertyIndex(9)]
        LessonType LessonType { get; set; }

        [PropertyIndex(10)]
        DateTime DateTime { get; set; }

        [PropertyIndex(11)]
        int State { get; set; }
    }
}